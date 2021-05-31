using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatButton : MonoBehaviour
{
    private CombatManager manager;
    [SerializeField] private PlayerState newState;
    [SerializeField] private GameObject cameraTarget;
    public AttackData attack = null;
    private GridManager gridManager = null;
    private PlayerEntity player;
    private TooltipUi attackTooltip;

    private void Start() {
        manager = CombatManager.current;
        player = manager.player;
        gridManager = manager.GetComponent<GridManager>();
    }

    public void ToggleState () {
        if (manager.playerState == PlayerState.Locked) return;
        if (manager.turn != Turn.PlayerTurn) return;
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
                }
                break;
            case PlayerState.Attacking :
                if (attack.CheckCost(player)){
                    if (attack.heal) {
                        CombatManager.current.player.hp += attack.dmg;
                        attack.Cost(CombatManager.current.player);
                    }
                    manager.playerState = PlayerState.Attacking;
                }
                break;
        }
    }

    public void RotateCamera (bool clockwise) {
        float rotation = cameraTarget.transform.eulerAngles.y;
        if (clockwise) {
            LeanTween.rotateY(cameraTarget,rotation+90f,0.8f).setEaseInOutQuint();
        }else{
            LeanTween.rotateY(cameraTarget,rotation-90f,0.8f).setEaseInOutQuint();
        }
    }
}
