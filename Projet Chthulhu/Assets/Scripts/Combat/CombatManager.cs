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
    public EntityManager gameEntities = new EntityManager();
    public TileEntity[,] grid;
    public PlayerState playerState = PlayerState.Normal;
    public GameObject player = null;
    private void Start() {
        foreach (GameObject g in gameEntities.entities) {
            if (g.GetComponent<ActorEntity>() != null) {
                player = g;
                break;
            }
        }
        Invoke("ResetActorsPositions",1f);
    }

    //toggle moving ui and let the player select a case
    public void TogglePlayerMovingState () {
        if (playerState == PlayerState.Moving) {
            playerState = PlayerState.Normal;
            GetComponent<GridMoveManager>().ResetHighlight();
        }else if (player.GetComponent<PlayerEntity>().pm > 0){
            playerState = PlayerState.Moving;
            GetComponent<GridMoveManager>().HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
        }else{
            playerState = PlayerState.Normal;
            GetComponent<GridMoveManager>().ResetHighlight();
        }
    }

    public void ResetActorsPositions() {
        foreach (TileEntity t in grid) {
            if (t == null || t.tileUser == null) { continue; }
            t.SetTileUser(null);
        }
        foreach (GameObject a in gameEntities.entities) {
            a.GetComponent<ActorEntity>().SnapToGrid();
        }
        GetComponent<GridMoveManager>().tileHighlightRanges.Clear();
    }

}
