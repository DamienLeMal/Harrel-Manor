using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public void AttackLaunch () {
        Debug.Log("Attack Launched event");
        CombatEventSystem.current.PlayerAttack();
    }
    public void AttackEnd () {
        CombatEventSystem.current.PlayerEndAttack();
    }
}
