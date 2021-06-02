using UnityEngine;
using System;

public class CombatEventSystem : MonoBehaviour
{
    public static CombatEventSystem current;

    private void Awake() {
        current = this;
    }
#region Events
    //Take Damage
    public event Action<int> onTakeDamage;
    public void TakeDamage(int damageAmount, ActorEntity actor) {
        if (onTakeDamage == null) return;
        if (actor == CombatManager.current.player) return;
        onTakeDamage(damageAmount);
    }

    //Deal Damage
    public event Action<int> onDealDamage;
    public void DealDamage(int damageAmount, ActorEntity actor) {
        if (onDealDamage == null) return;
        if (actor == CombatManager.current.player) return;
        onDealDamage(damageAmount);
    }

    //Update player Hp
    public event Action onHpChange;
    public void HpChange(ActorEntity actor) {
        if (onHpChange == null) return;
        if (actor != CombatManager.current.player) return;
        onHpChange();
    }

    //Player is locked
    public event Action onPlayerLocked;
    public void PlayerLocked () {
        if (onPlayerLocked == null) return;
        onPlayerLocked();
    }

    public event Action onPlayerUnlocked;
    public void PlayerUnlocked () {
        if (onPlayerUnlocked == null) return;
        onPlayerUnlocked();
    }

    public event Action onPlayerTurn;
    public void PlayerTurn () {
        if (onPlayerTurn == null) return;
        onPlayerTurn();
    }

    public event Action onEnnemyTurn;
    public void EnnemyTurn () {
        if (onEnnemyTurn == null) return;
        onEnnemyTurn();
    }

    public event Action onPlayerAttack;
    public void PlayerAttack () {
        if (onPlayerAttack == null) return;
        onPlayerAttack();
    }

    public event Action onPlayerEndAttack;
    public void PlayerEndAttack () {
        if (onPlayerEndAttack == null) return;
        onPlayerEndAttack();
    }

#endregion
}
