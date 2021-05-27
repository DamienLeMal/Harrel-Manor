using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject background;
    [SerializeField] private AttackUiManager attackUiManager;
    [SerializeField] private int selectedIndex;
    private PlayerEntity player;
    private bool deployed = false;

    private void Start() {
        player = CombatManager.current.player;
    }

    public void WeaponDropDown () {
        if(!deployed) {
            for (int i = 0; i < weapons.Length; i++) {
                if (player.weaponInventory.Count > i) {
                    weapons[i].SetActive(true);
                    weapons[i].name = player.weaponInventory[i].name;
                    weapons[i].GetComponent<Image>().sprite = player.weaponInventory[i].weaponIcon;
                }
            }
            background.SetActive(true);
            background.transform.localScale = new Vector3(1,0.1f,1);
            LeanTween.scaleY(background,1,0.2f);
        }else{
            foreach(GameObject g in weapons) {
                g.name = "-";
                background.transform.localScale = Vector3.one;
                LeanTween.scaleY(background,0.1f,0.2f);
                Invoke("SetActive",0.2f);
            }
        }
        deployed = !deployed;
    }

    private void SetActive () {
        foreach (GameObject g in weapons) {
            g.SetActive(false);
        }
        background.SetActive(false);
    }

    public void SelectWeapon (int index) {
        WeaponDropDown();
        attackUiManager.LinkAttackAndButton(player.weaponInventory[index]);
        //Set icon here
    }
}
