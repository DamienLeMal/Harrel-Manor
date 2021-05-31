using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnnemyEntity : ActorEntity
{

    private void Start() {
        level = GetRealLevel();
        Debug.Log(level);
    }


    /// <summary>
    /// Make the ennemy disapear
    /// </summary>
    override protected void ActorDeath () {
        Destroy(gameObject);
        manager.turnManager.fightingEntities.Remove(this);
        manager.ResetActorsPositions();
        manager.turnManager.TestCombatEnd();
        StartCoroutine(GivePlayerXp());
    }

    private IEnumerator GivePlayerXp () {
        int xpGain = CalculateExpGain();
        string popupText = "Vous avec vaincu " + entityName + "\nVous avez gagné " + xpGain.ToString() + " xp !";
        yield return StartCoroutine(manager.popup.StartPopup(popupText,"Level Up",PopupType.Information,GivePlayerXpEnd));
        
    }

    private void GivePlayerXpEnd () {
        manager.player.AddXpGain(CalculateExpGain());
        Debug.Log("after coroutine");
    }

    private int CalculateExpGain () {
        int xpGain = manager.player.realLevel/level*60;
        return xpGain;
    }

}
