using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : ActorEntity
{
    private int exp = 0;
    private int exp_goal;
    [HideInInspector] public int realLevel;
    private int amountLeft;

    private void Start() {
        exp_goal = level * 10;
        realLevel = GetRealLevel();
    }

    public ActorDialogue GetDialogue() {
        return baseStats.dialogue;
    }

    public void AddXpGain (int amount) {
        //if exp + xp gain > exp goal -> LevelUp()
        //Else apply
        if (exp + amount >= exp_goal) {
            Debug.Log("Level Up");
            LevelUp(exp + amount - exp_goal);
            return;
        }
        Debug.Log("Not enough xp");
        exp+=amount;
    }

    private void LevelUp (int amountRemaining) {
        //level up -> popup -> upgrade some stats
        level++;
        exp_goal = level * 10;
        amountLeft = amountRemaining;
        string popupText = "Vous gagnez un niveau !\nVous êtes niveau " + level.ToString() + " !\nSélectionnez une stat à améliorer";
        StartCoroutine(manager.popup.ActivatePopup(popupText,PopupType.LevelUp,LevelUpEnd));
    }
    private void LevelUpEnd() {
        //add remaining xp gain -> add XpGain(reste)
        AddXpGain(amountLeft);
    }

    public void UpgradeStat (PlayerStat pStat) {
        int upgradeAmount = Random.Range(1,14);
        switch (pStat) {
            case PlayerStat.Str :
                str += ProcessUpgradeAmount(str, upgradeAmount);
                break;
            case PlayerStat.Dex :
                dex += ProcessUpgradeAmount(dex, upgradeAmount);
                break; 
            case PlayerStat.Spd :
                spd += ProcessUpgradeAmount(spd, upgradeAmount);
                break; 
            case PlayerStat.Intl :
                intl += ProcessUpgradeAmount(intl, upgradeAmount);
                break;
            case PlayerStat.Agi :
                agi += ProcessUpgradeAmount(agi, upgradeAmount);
                break; 
            case PlayerStat.Con :
                con += ProcessUpgradeAmount(con, upgradeAmount);
                break; 
            case PlayerStat.Lck :
                lck += ProcessUpgradeAmount(lck, upgradeAmount);
                break; 
        }
        realLevel = GetRealLevel();
    }

    private int ProcessUpgradeAmount (int stat, int upgradeAmount) {
        if (Random.Range(0,100) > stat) {
            return CheckIfUnderHundred(stat,upgradeAmount/2);
        }
        return CheckIfUnderHundred(stat,upgradeAmount);
    }

    private int CheckIfUnderHundred (int stat, int amount) {
        if (stat+amount <= 100) return amount;
        return 100-stat;
    }
}
