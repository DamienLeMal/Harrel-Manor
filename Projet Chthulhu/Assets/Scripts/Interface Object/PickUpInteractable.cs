using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : MonoBehaviour, IClicked
{
    [SerializeField] WeaponData weapon;
    public void OnClickAction () {
        CombatManager.current.player.AddNewWeapon(weapon);
        Destroy(gameObject);
    }
}
