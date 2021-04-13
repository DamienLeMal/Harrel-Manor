using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatButton : MonoBehaviour
{
    [SerializeField] private CombatManager manager = null;
    [SerializeField] private PlayerState newState;
    [SerializeField] private AttackData attack = null;
    //public void ToggleState () {
    //    //Reset back to normal
    //    manager.playerState = PlayerState.Normal;
    //    gridManager.ResetHighlight();
    //    if (playerState == newState) { return; }//Stay on normal
    //    switch (newState) {
    //        case PlayerState.Moving :
    //            if (player.GetComponent<PlayerEntity>().pm > 0) {
    //                playerState = PlayerState.Moving;
    //                gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
    //            }
    //            break;
    //        case PlayerState.Attacking :
    //            if (player.GetComponent<PlayerEntity>().ap >= attack.apCost && player.GetComponent<PlayerEntity>().mp >= attack.mpCost){
    //                playerState = PlayerState.Attacking;
    //                gridManager.HighlightSurroundingTiles(player.GetComponent<PlayerEntity>().currentTile);
    //            }
    //            break;
    //    }
    //}
}
