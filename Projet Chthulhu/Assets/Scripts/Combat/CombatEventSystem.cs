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

#endregion
}
