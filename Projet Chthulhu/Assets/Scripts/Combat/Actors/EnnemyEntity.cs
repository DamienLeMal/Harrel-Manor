using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnnemyEntity : ActorEntity
{

    private void Start() {
        level = GetRealLevel();
    }

    /// <summary>
    /// Make the ennemy disapear
    /// </summary>
    override protected void ActorDeath () {
        Debug.Log("Ennemy Death");
        base.ActorDeath();
        Destroy(gameObject);
        manager.turnManager.fightingEntities.Remove(this);
        manager.ResetActorsPositions();
        manager.turnManager.TestCombatEnd();
        StartCoroutine(GivePlayerXp());
    }

    private IEnumerator GivePlayerXp () {
        Debug.Log("Give player xp");
        int xpGain = CalculateExpGain();
        string popupText = "Vous avec vaincu " + entityName + "\nVous avez gagné " + xpGain.ToString() + " xp !";
        yield return StartCoroutine(manager.popup.StartPopup(popupText,"Level Up",PopupType.Information,GivePlayerXpEnd));
        
    }

    private void GivePlayerXpEnd () {
        manager.player.AddXpGain(CalculateExpGain());
    }

    private int CalculateExpGain () {
        int xpGain = manager.player.realLevel/level*60;
        return xpGain;
    }

}
