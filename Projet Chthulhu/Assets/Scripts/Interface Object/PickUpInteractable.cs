using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : MonoBehaviour, IClicked
{
    [SerializeField] WeaponData weapon;
    private InterfaceDistance interfaceDistance;
    private AudioSource source;

    private void Start() {
        source = GetComponent<AudioSource>();
        interfaceDistance = GetComponent<InterfaceDistance>();
    }
    public void OnClickAction () {
        if (source != null) source.Play();
        if (!interfaceDistance.isInteractable) return;
        CombatManager.current.player.AddNewWeapon(weapon);
        Destroy(gameObject);
    }
}
