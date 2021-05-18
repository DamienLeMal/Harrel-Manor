using System;
using UnityEngine;

public class SoundEventManager : MonoBehaviour
{
    public static SoundEventManager current;
    private void Awake() {
        current = this;
    }

#region Events

    //Move
    public event Action onActorMove;
    public void ActorMove() {
        if (onActorMove == null) return;
        onActorMove();
    }

    //Attack
    public event Action onAttackLaunch;
    public void AttackLaunch() {
        if (onAttackLaunch == null) return;
        onAttackLaunch();
    }

    //Being Attacked
    public event Action onActorBeingAttacked;
    public void ActorBeingAttacked() {
        if (onActorBeingAttacked == null) return;
        onActorBeingAttacked();
    }

    //Interaction Object
    public event Action onInteract;
    public void Interact() {
        if (onInteract == null) return;
        onInteract();
    }

    //Close to trigger object
    public event Action onCloseToTrigger;
    public void CloseToTrigger() {
        if (onCloseToTrigger == null) return;
        onCloseToTrigger();
    }

    //Actor's Death
    public event Action onActorDeath;
    public void ActorDeath() {
        if (onActorDeath == null) return;
        onActorDeath();
    }

    //Random
    public event Action onRandomEvent;
    public void RandomEvent() {
        if (onRandomEvent == null) return;
        onRandomEvent();
    }

    //Ennemy in sight
    public event Action onEnnemyInSight;
    public void EnnemyInSight() {
        if (onEnnemyInSight == null) return;
        onEnnemyInSight();
    }

    //Changing gamemode
    public event Action onGamemodeChange;
    public void GamemodeChange() {
        CombatManager.current.combatOn = !CombatManager.current.combatOn;
        if (onGamemodeChange == null) return;
        onGamemodeChange();
    }

    //New player's turn and low hp
    public event Action onPlayerTurn;
    public void PlayerTurn() {
        if (onPlayerTurn == null) return;
        onPlayerTurn();
    }

    //End of Combat
    public event Action<bool> onCombatEnd;
    public void CombatEnd(bool playerWin) {
        if (onCombatEnd == null) return;
        onCombatEnd(playerWin);
    }

    //Pause
    public event Action onPause;
    public void Pause() {
        if (onPause == null) return;
        onPause();
    }
    //Unpause
    public event Action onUnpause;
    public void Unpause() {
        if (onUnpause == null) return;
        onUnpause();
    }

    public event Action<int> onDialogue;
    public void Dialogue (int id) {
        if (onDialogue == null) return;
        onDialogue(id);
    }
#endregion
}
