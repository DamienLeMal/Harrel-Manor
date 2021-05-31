using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Combat/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    string weaponName;
    public Sprite weaponIcon;
    public List<AttackData> attacks;
}
