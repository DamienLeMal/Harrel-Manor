using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatButton : MonoBehaviour
{
    [SerializeField] private CombatManager manager = null;
    [SerializeField] private PlayerState newState;
    public AttackData attack = null;
    private GridManager gridManager = null;
    private PlayerEntity player;

    public void ButtonConstructor (CombatManager _manager, PlayerState _newState, AttackData _attack) {
        manager = _manager;
        newState = _newState;
        attack = _attack;
        GetComponent<Button>().onClick.AddListener(ToggleState);
    }

    private void Start() {
        gridManager = manager.GetComponent<GridManager>();
    }

    public void ToggleState () {
        if (player == null) {
            player = manager.player.GetComponent<PlayerEntity>();
        }
        //Reset grid
        gridManager.ResetHighlight();
        //Toggle Deactivate
        if (manager.playerState == newState && manager.activeButton == this) { 
            manager.playerState = PlayerState.Normal;
            return; 
        }
        //Toggle Activate
        manager.activeButton = this;
        switch (newState) {
            case PlayerState.Moving :
                if (player.pm > 0) {
                    manager.playerState = PlayerState.Moving;
                    gridManager.HighlightActionTiles(player.currentTile);
                }
                break;
            case PlayerState.Attacking :
                if (player.ap >= attack.apCost && player.mp >= attack.mpCost){
                    manager.playerState = PlayerState.Attacking;
                    gridManager.HighlightActionTiles(player.currentTile);
                }
                break;
        }
    }
}
