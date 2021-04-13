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
    private void Start() {
        gridManager = GetComponent<GridManager>();
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<ActorEntity>() != null) {
                player = g;
                break;
            }
        }
        Invoke("ResetActorsPositions",1f);
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

    //toggle moving ui and let the player select a case
    public void TogglePlayerMovingState () {
        if (playerState == PlayerState.Moving) {
            playerState = PlayerState.Normal;
            gridManager.ResetHighlight();
        }else if (player.GetComponent<PlayerEntity>().pm > 0){
            playerState = PlayerState.Moving;
            gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
        }else{
            playerState = PlayerState.Normal;
            gridManager.ResetHighlight();
        }
    }

    //Temp
    public void ToggleAttack () {
        if (playerState == PlayerState.Attacking) {
            playerState = PlayerState.Normal;
            gridManager.ResetHighlight();
        }else if (player.GetComponent<PlayerEntity>().ap >= player.GetComponent<PlayerEntity>().weaponInvetory[0].attacks[0].apCost && player.GetComponent<PlayerEntity>().mp >= player.GetComponent<PlayerEntity>().weaponInvetory[0].attacks[0].mpCost){
            playerState = PlayerState.Attacking;
            gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
        }else{
            playerState = PlayerState.Normal;
            gridManager.ResetHighlight();
        }
    }
    
    public void ToggleState (PlayerState newState, AttackData attack = null) {
        //Reset back to normal
        playerState = PlayerState.Normal;
        gridManager.ResetHighlight();
        if (playerState == newState) { return; }//Stay on normal
        switch (newState) {
            case PlayerState.Moving :
                if (player.GetComponent<PlayerEntity>().pm > 0) {
                    playerState = PlayerState.Moving;
                    gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
                }
                break;
            case PlayerState.Attacking :
                if (player.GetComponent<PlayerEntity>().ap >= attack.apCost && player.GetComponent<PlayerEntity>().mp >= attack.mpCost){
                    playerState = PlayerState.Attacking;
                    gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
                }
                break;
        }
    }
}
