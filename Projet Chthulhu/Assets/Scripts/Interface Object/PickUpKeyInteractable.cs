using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpKeyInteractable : MonoBehaviour, IClicked
{
    [SerializeField] private int keyId;
    private InterfaceDistance interfaceDistance;

    private string id;

    private void Start() {
        interfaceDistance = GetComponent<InterfaceDistance>();
        SaveSystem.current.pickUp.Add(gameObject);
        id = "pkupkey_" + SceneManager.GetActiveScene().name + "_" + SaveSystem.current.pickUp.IndexOf(gameObject).ToString();
        Debug.Log(id + " & " + PlayerPrefs.GetInt(id));

        if (PlayerPrefs.GetInt(id) == 2) {
            Destroy(gameObject);
            return;
        } 
        PlayerPrefs.SetInt(id,1);
        PlayerPrefs.Save();
    }
    public void OnClickAction () {
        if (!interfaceDistance.isInteractable) return;
        CombatManager.current.player.AddKey(keyId);
        PlayerPrefs.SetInt(id,2);
        Destroy(gameObject);
    }
}
