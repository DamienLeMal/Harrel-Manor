using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDropWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private PopupWindow popupWindow;
    private Dictionary<GameObject,WeaponData> assignedButtons;
    public void AssignWeaponToButton (WeaponData pickedUpWeapon) {
        assignedButtons = new Dictionary<GameObject, WeaponData>();
        assignedButtons.Add(buttons[0],CombatManager.current.player.weaponInventory[0]);
        assignedButtons.Add(buttons[1],CombatManager.current.player.weaponInventory[1]);
        assignedButtons.Add(buttons[2],CombatManager.current.player.weaponInventory[2]);
        assignedButtons.Add(buttons[3],pickedUpWeapon);
    }

    public void ButtonAction (GameObject button) {
        Time.timeScale = 1;
        popupWindow.UnactivatePopup();
        if (button != buttons[3]) {
            Debug.Log(assignedButtons[button]);
            Debug.Log(assignedButtons[buttons[3]]);

            CombatManager.current.player.ReplaceWeapon(assignedButtons[button],assignedButtons[buttons[3]]);
        }
    }
}
