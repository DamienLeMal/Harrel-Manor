using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Combat/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    string weaponName;
    public List<AttackData> attacks;
}
