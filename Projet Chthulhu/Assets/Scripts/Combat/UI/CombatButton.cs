using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CombatManager manager = null;
    [SerializeField] private PlayerState newState;
    [SerializeField] private GameObject cameraTarget;
    public AttackData attack = null;
    private GridManager gridManager = null;
    private PlayerEntity player;
    private TooltipUi attackTooltip;

    public void ButtonConstructor (CombatManager _manager, PlayerState _newState, AttackData _attack, TooltipUi _attackTooltip) {
        manager = _manager;
        newState = _newState;
        attack = _attack;
        attackTooltip = _attackTooltip;
        GetComponent<Button>().onClick.AddListener(ToggleState);
    }

    private void Start() {
        gridManager = manager.GetComponent<GridManager>();
    }

    public void ToggleState () {
        if (player == null) {
            player = manager.player;
        }
        if (manager.playerState == PlayerState.Locked) return;
        if (manager.turn != Turn.PlayerTurn) return;
        //Reset grid
        //gridManager.ResetTileHighlight();
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
                    //gridManager.HighlightActionTiles();
                }
                break;
            case PlayerState.Attacking :
                if (player.ap >= attack.apCost && player.mp >= attack.mpCost){
                    manager.playerState = PlayerState.Attacking;
                    //gridManager.HighlightActionTiles();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (newState != PlayerState.Attacking) return;
        attackTooltip.gameObject.SetActive(true);
        attackTooltip.SetText(attack.attackName, attack.description);
        attackTooltip.SetTooltipPosition(transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(attackTooltip.GetComponentInParent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (newState != PlayerState.Attacking) return;
        attackTooltip.gameObject.SetActive(false);
    }
}
