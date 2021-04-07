using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Normal,
    Moving,
    Attacking
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
    }

    //toggle moving ui and let the player select a case
    public void TogglePlayerMovingState () {
        if (playerState == PlayerState.Moving) {
            playerState = PlayerState.Normal;
        }else if (player.GetComponent<PlayerEntity>().pm > 0){
            playerState = PlayerState.Moving;
            GetComponent<GridMoveManager>().HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
        }else{
            playerState = PlayerState.Normal;
        }
    }

    //RequestMove(Actor) run the algorythme and send the actor to the location
}
