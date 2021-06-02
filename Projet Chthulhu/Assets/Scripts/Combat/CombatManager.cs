using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Normal,
    Moving,
    Attacking,
    Locked
}
public class CombatManager : MonoBehaviour
{
    public static CombatManager current;
    public bool combatOn = false;
    [HideInInspector] public EntityManager gameEntities = new EntityManager();
    public Turn turn = Turn.Start;
    [HideInInspector] public CombatTurnManager turnManager;
    [HideInInspector] public CombatUiManager uiManager;

    [HideInInspector] public TileEntity[,] grid;
    /// <summary>
    /// Array of Player States that affect the Grid
    /// </summary>
    [HideInInspector] public PlayerState[] pStateAffectGrid = {PlayerState.Moving, PlayerState.Attacking};
    [HideInInspector] public PlayerEntity player = null;
    [HideInInspector] public GridManager gridManager = null;
    [SerializeField] private float maxCombatDistance;
    public PlayerState playerState {
        get {return _playerState;}
        set {
            _playerState = value;
            UpdateTileHighlight();
        }
    }
    private PlayerState _playerState = PlayerState.Normal;
    public PopupWindow popup = null;
    //Editor Materials
    public Material walkMaterial;
    public Material blockMaterial;
    public Material occupiedMaterial;
    public CombatParticleManager particleManager;

    [HideInInspector] public CombatButton activeButton = null;
    private void Awake() {
        current = this;
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<PlayerEntity>() != null) {
                player = g.GetComponent<PlayerEntity>();
                break;
            }
        }
        gameEntities.SetEnnemiesId();
    }
    private void Start() {
        uiManager = GetComponent<CombatUiManager>();
        gridManager = GetComponent<GridManager>();
        turnManager = GetComponent<CombatTurnManager>();
        particleManager = GetComponent<CombatParticleManager>();
        SoundEventManager.current.onCombatEnd += EndCombatMode;
    }
    public void ResetActorsPositions() {
        foreach (TileEntity t in grid) {
            if (t == null || t.tileUser == null) { continue; }
            t.SetTileUser(null);
        }
        foreach (ActorEntity a in turnManager.fightingEntities) {
            a.SnapToGrid();
        }
        gridManager.ClearTileHighlight();
    }

    public void StartCombat (ActorEntity actorPriority, EnnemyEntity firstEnnemy) {
        uiManager.ToggleCombatUi(true);
        turnManager.fightingEntities = new List<ActorEntity>();
        turnManager.fightingEntities.Add(actorPriority);
        if (!turnManager.fightingEntities.Contains(firstEnnemy)) turnManager.fightingEntities.Add(firstEnnemy);
        foreach (TileEntity t in grid) {
            if (t == null) continue;
            t.gameObject.SetActive(true);
            t.UpdateMaterial();
        }
        foreach (GameObject g in gameEntities.entities) {
            if (Vector3.Distance(player.transform.position,g.transform.position) > maxCombatDistance) {
                if (!turnManager.fightingEntities.Contains(g.GetComponent<ActorEntity>())) {
                    g.SetActive(false);//Ennemies not in combat are unactivated
                    continue;
                }
            }
            g.GetComponent<ActorEntity>().animator.SetBool("isWalking",false);
            if (turnManager.fightingEntities.Contains(g.GetComponent<ActorEntity>())) continue;
            turnManager.fightingEntities.Add(g.GetComponent<ActorEntity>());
        }
        ResetActorsPositions();
        turnManager.NewTurn();
    }

    public void EndCombatMode (bool playerWin) {
        if (!playerWin) return;
        player.hp += player.hp_max/4;
        //Set everything off and player exploration mode on
        playerState = PlayerState.Locked;
        foreach (TileEntity t in grid) {
            if (t == null) continue;
            t.gameObject.SetActive(false);
        }

//Not Tested

        //Re-activate all ennemies that were not in combat
        foreach (GameObject g in gameEntities.entities) {
            g.SetActive(true);
        }

//End Not Tested

        uiManager.ToggleCombatUi(false);
        player.GetComponent<PlayerDeplacement>().SetExplorationMode();
        SoundEventManager.current.GamemodeChange();
    }

    private void UpdateTileHighlight () {
        gridManager.ResetTileHighlight();
        gridManager.HighlightActionTiles();
    }

    public void GameOver() {
        //Temp
        player.gameObject.SetActive(false);

        uiManager.GameOverScreen();
        SoundEventManager.current.CombatEnd(false);
    }
}
