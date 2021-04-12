using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Combat/Actor", order = 0)]
public class ActorData : ScriptableObject
{
    public int str, dex, spd, intl, agi, con, lck, mnt, pm_max, ap_max, mp_max;
    public List<WeaponData> weaponInvetory;
    //public Spell[] spellInventory;
    //public Armor[] armorInvetory;
}