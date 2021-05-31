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

    [HideInInspector] public CombatButton activeButton = null;
    private void Awake() {
        current = this;
    }
    private void Start() {
        SoundEventManager.current.onGamemodeChange += OnGamemodeChange;
        uiManager = GetComponent<CombatUiManager>();
        gridManager = GetComponent<GridManager>();
        turnManager = GetComponent<CombatTurnManager>();
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<PlayerEntity>() != null) {
                player = g.GetComponent<PlayerEntity>();
                break;
            }
        }
        gameEntities.SetEnnemiesId();
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

    public void StartCombat (ActorEntity actorPriority) {
        uiManager.ToggleCombatUi(true);
        turnManager.fightingEntities = new List<ActorEntity>();
        turnManager.fightingEntities.Add(actorPriority);
        foreach (TileEntity t in grid) {
            if (t == null) continue;
            t.gameObject.SetActive(true);
        }
        foreach (GameObject g in gameEntities.entities) {
            if (Vector3.Distance(player.transform.position,g.transform.position) > 123456789) continue;
            if (turnManager.fightingEntities.Contains(g.GetComponent<ActorEntity>())) continue;
            turnManager.fightingEntities.Add(g.GetComponent<ActorEntity>());
        }
        ResetActorsPositions();
        foreach (WeaponData w in player.weaponInventory) {
            Transform wb = uiManager.ShowWeaponButton().attackContainer;
            foreach(AttackData a in w.attacks) {
                GetComponent<CombatUiManager>().ShowAttackButton(a,wb);
            }
        }
        turnManager.NewTurn();
    }

    public void EndCombatMode () {
        //Set everything off and player exploration mode on
        playerState = PlayerState.Locked;
        foreach (TileEntity t in grid) {
            if (t == null) continue;
            t.gameObject.SetActive(false);
        }
        uiManager.ToggleCombatUi(false);
        player.GetComponent<PlayerDeplacement>().SetExplorationMode();
        SoundEventManager.current.GamemodeChange();
    }

    private void UpdateTileHighlight () {
        gridManager.ResetTileHighlight();
        gridManager.HighlightActionTiles();
    }

    private void OnGamemodeChange () {
        Debug.Log("gameModeChange");
    }
}
