using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : ActorEntity
{
    private List<int> keys = new List<int>();
    private int exp = 0;
    private int exp_goal;
    [HideInInspector] public int realLevel;
    private int amountLeft;

    private void Start() {
        Debug.Log(hp + " & " + mnt);
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
            LevelUp(exp + amount - exp_goal);
            return;
        }
        exp+=amount;
    }

    private void LevelUp (int amountRemaining) {
        //level up -> popup -> upgrade some stats
        level++;
        exp_goal = level * 10;
        amountLeft = amountRemaining;
        string popupText = "Vous gagnez un niveau !\nVous êtes niveau " + level.ToString() + " !\nSélectionnez une stat à améliorer";
        manager.popup.ActivatePopup(popupText,"Level Up",PopupType.LevelUp,LevelUpEnd);
        //StartCoroutine(manager.popup.ActivatePopup(popupText,PopupType.LevelUp,LevelUpEnd));
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

    protected override void ActorDeath()
    {
        base.ActorDeath();
        //GameOver
        manager.GameOver();
    
    }

    public void AddNewWeapon (WeaponData weapon) {
        if (weaponInventory.Count < 3) {
            weaponInventory.Add(weapon);
            return;
        }
        //Open Popup
        manager.popup.ActivatePopup("","Choisissez une arme à laisser",PopupType.DropWeapon);
        manager.popup.PrepareDropWeapon(weapon);
    }

    public void ReplaceWeapon (WeaponData weaponToRemove, WeaponData weaponToAdd) {
        Debug.Log("Weapon replaced");
        weaponInventory.Remove(weaponToRemove);
        weaponInventory.Add(weaponToAdd);
    }

    public void AddKey (int id) {
        keys.Add(id);
    }

    public bool HasKey (int id) {
        return keys.Contains(id);
    }
}
