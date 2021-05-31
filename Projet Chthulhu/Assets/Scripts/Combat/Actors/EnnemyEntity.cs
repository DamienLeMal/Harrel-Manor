using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnnemyEntity : ActorEntity
{
    public string id;
    public override void Start() {
        base.Start();
        level = GetRealLevel();

        //Save or Destroy
        if (PlayerPrefs.GetInt(id) == 2) {
            DestroyEnnemy();
            return;
        } 
            PlayerPrefs.SetInt(id,1);
            PlayerPrefs.Save();
    }


    /// <summary>
    /// Make the ennemy disapear
    /// </summary>
    override protected void ActorDeath () {
        PlayerPrefs.SetInt(id,2);
        PlayerPrefs.Save();
        Debug.Log("Ennemy Death");
        base.ActorDeath();
        manager.turnManager.fightingEntities.Remove(this);
        DestroyEnnemy();
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

    private void DestroyEnnemy () {
        manager.gameEntities.entities.Remove(gameObject);
        Destroy(gameObject);
    }
}
