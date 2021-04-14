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
    /// <summary>
    /// Array of Player States that affect the Grid
    /// </summary>
    [HideInInspector] public PlayerState[] pStateAffectGrid = {PlayerState.Moving, PlayerState.Attacking};
    [HideInInspector] public GameObject player = null;
    private GridManager gridManager = null;
    public CombatButton activeButton = null;
    private void Start() {
        gridManager = GetComponent<GridManager>();
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<ActorEntity>() != null) {
                player = g;
                break;
            }
        }
        Invoke("StartCombat",1f);
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

    public void StartCombat () {
        ResetActorsPositions();
        foreach (WeaponData w in player.GetComponent<PlayerEntity>().weaponInvetory) {
            foreach(AttackData a in w.attacks) {
                GetComponent<CombatUiManager>().ShowAttackButton(a);
            }
        }
    }
}
