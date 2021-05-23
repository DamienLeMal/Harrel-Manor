using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private float[] heightPose;
    private PlayerEntity player;
    private bool deployed = false;

    private void Start() {
        player = CombatManager.current.player;
        heightPose = new float[weapons.Length];
        InitialiseHeightPose();

    }

    private void InitialiseHeightPose () {
        float sizeY = weapons[0].GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log(sizeY);
        for (int i = 0; i < weapons.Length; i++) {
            heightPose[i] = sizeY * (i+1) + 10*i +10;
        }
    }

    public void WeaponDropDown () {
        if(!deployed) {
            for (int i = 0; i < weapons.Length; i++) {
                if (player.weaponInventory.Count > i) {
                    weapons[i].SetActive(true);
                    weapons[i].name = player.weaponInventory[i].name;
                    LeanTween.moveLocalY(weapons[i],heightPose[i],0.2f);
                }
            }
        }else{
            foreach(GameObject g in weapons) {
                g.name = "-";
                LeanTween.moveLocalY(g,0,0.2f);
                Invoke("SetActive",0.2f);
            }
        }
        deployed = !deployed;
    }

    private void SetActive () {
        foreach (GameObject g in weapons) {
            g.SetActive(false);
        }
    }
}
