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
    public TileEntity[,] grid;
    public PlayerState playerState = PlayerState.Normal;
    public Turn turn = Turn.Start;
    /// <summary>
    /// Array of Player States that affect the Grid
    /// </summary>
    [HideInInspector] public PlayerState[] pStateAffectGrid = {PlayerState.Moving, PlayerState.Attacking};
    [HideInInspector] public PlayerEntity player = null;
    private GridManager gridManager = null;
    public CombatButton activeButton = null;
    private void Start() {
        gridManager = GetComponent<GridManager>();
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<PlayerEntity>() != null) {
                player = g.GetComponent<PlayerEntity>();
                break;
            }
        }
        Invoke("InvokeCombat",1f);
    }

    //TEMP
    private void InvokeCombat () {
        StartCombat(player);
    }
    public void ResetActorsPositions() {
        foreach (TileEntity t in grid) {
            if (t == null || t.tileUser == null) { continue; }
            t.SetTileUser(null);
        }
        foreach (GameObject a in gameEntities.entities) {
            a.GetComponent<ActorEntity>().SnapToGrid();
        }
        gridManager.ClearTileHighlight();
    }

    public void StartCombat (ActorEntity actorPriority) {
        CombatTurnManager turnManager = GetComponent<CombatTurnManager>();
        turnManager.fightingEntities = new List<ActorEntity>();
        turnManager.fightingEntities.Add(actorPriority);
        foreach (GameObject g in gameEntities.entities) {
            if (Vector3.Distance(player.transform.position,g.transform.position) > 123456789) continue;
            if (turnManager.fightingEntities.Contains(g.GetComponent<ActorEntity>())) continue;
            turnManager.fightingEntities.Add(g.GetComponent<ActorEntity>());
        }
        ResetActorsPositions();
        foreach (WeaponData w in player.weaponInventory) {
            foreach(AttackData a in w.attacks) {
                GetComponent<CombatUiManager>().ShowAttackButton(a);
            }
        }
        turnManager.NewTurn();
    }
}
