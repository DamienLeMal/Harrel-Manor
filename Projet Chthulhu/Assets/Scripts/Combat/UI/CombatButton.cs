using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatButton : GameButton
{
    private CombatManager manager;
    [SerializeField] private PlayerState newState;
    public AttackData attack = null;
    private GridManager gridManager = null;
    private PlayerEntity player;

    private void Start() {
        manager = CombatManager.current;
        player = manager.player;
        gridManager = manager.GetComponent<GridManager>();
        CombatEventSystem.current.onPlayerLocked += LockButton;
        CombatEventSystem.current.onPlayerUnlocked += UnlockButton;
        CombatEventSystem.current.onEnnemyTurn += LockButton;
        CombatEventSystem.current.onPlayerTurn += UnlockButton;
        CombatEventSystem.current.onPlayerEndAttack += LockAttack;
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
    private void LockAttack () {
        if (!CanAttack() && attack != null) LockButton();
    }

    private void LockButton () {
        TemporaryUnactivate(GetComponent<Button>());
    }

    private void UnlockButton () {
        if (manager.playerState == PlayerState.Locked) return;
        if (manager.turn != Turn.PlayerTurn) return;
        if (!CanAttack()) return;
        ReactivateButton(GetComponent<Button>());
    }

    private bool CanAttack () {
        if (attack == null) return true;
        return attack.CheckCost(player);
    }
}
