using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Combat/Actor", order = 0)]
public class ActorData : ScriptableObject
{
    public int str, dex, spd, intl, agi, con, lck, mnt;
    //public Weapon[] weaponInvetory;
    //public Spell[] spellInventory;
    //public Armor[] armorInvetory;
}