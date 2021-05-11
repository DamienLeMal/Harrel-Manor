using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : ActorEntity
{
    private int exp;
    private int exp_goal;

    private void CalculateXpGain (int ennemyLvl) {
        //Calculate xp gain

        
    }

    private void AddXpGain (int amount) {
        //if exp + xp gain > exp goal -> LevelUp()
        //Else apply
    }

    private void LevelUp (int amountRemaining) {
        //level up -> popup -> upgrade some stats
        
        
        
        //add remaining xp gain -> add XpGain(reste)
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
