using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpInteractable : MonoBehaviour, IClicked
{
    [SerializeField] WeaponData weapon;
    private InterfaceDistance interfaceDistance;
    private AudioSource source;
    private string id;

    private void Start() {
        source = GetComponent<AudioSource>();
        interfaceDistance = GetComponent<InterfaceDistance>();
        SaveSystem.current.pickUp.Add(gameObject);
        id = "pkupwp_" + SceneManager.GetActiveScene().name + "_" + SaveSystem.current.pickUp.IndexOf(gameObject).ToString();
        Debug.Log(id + " & " + PlayerPrefs.GetInt(id));

        if (PlayerPrefs.GetInt(id) == 2) {
            Destroy(gameObject);
            return;
        } 
        PlayerPrefs.SetInt(id,1);
        PlayerPrefs.Save();
    }
    public void OnClickAction () {
        if (source != null) source.Play();
        if (!interfaceDistance.isInteractable) return;
        CombatManager.current.player.AddNewWeapon(weapon);
        PlayerPrefs.SetInt(id,2);
        Destroy(gameObject);
    }
}
