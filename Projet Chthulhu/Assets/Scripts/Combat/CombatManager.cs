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
    [HideInInspector] public EntityManager gameEntities = new EntityManager();
    public PlayerState playerState = PlayerState.Normal;
    
    public Turn turn = Turn.Start;
    public CombatTurnManager turnManager;
    public CombatUiManager uiManager;

    public TileEntity[,] grid;
    /// <summary>
    /// Array of Player States that affect the Grid
    /// </summary>
    [HideInInspector] public PlayerState[] pStateAffectGrid = {PlayerState.Moving, PlayerState.Attacking};
    [HideInInspector] public PlayerEntity player = null;
    public GridManager gridManager = null;

    public CombatButton activeButton = null;
    private void Start() {
        uiManager = GetComponent<CombatUiManager>();
        gridManager = GetComponent<GridManager>();
        turnManager = GetComponent<CombatTurnManager>();
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<PlayerEntity>() != null) {
                player = g.GetComponent<PlayerEntity>();
                break;
            }
        }
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
        turnManager.fightingEntities = new List<ActorEntity>();
        turnManager.fightingEntities.Add(actorPriority);
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
        player.GetComponent<PlayerDeplacement>().SetExplorationMode();
    }
}
