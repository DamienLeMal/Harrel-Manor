using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PopupType {
    Information,
    LevelUp,
    DropWeapon
}
public class PopupWindow : MonoBehaviour
{
    [SerializeField] private GameObject popupWindow;
    [SerializeField] private Text title;
    [SerializeField] private Text txt;
    [SerializeField] private GameObject infoButton;
    [SerializeField] private GameObject lvlUpButtons;
    [SerializeField] private GameObject dropWeaponButtons;
    private bool popupActivated = false;
    private Action afterCoroutineMethod;
    

    private void Start() {
        //ActivatePopup("text", PopupType.Information);
    }

    public void ActivatePopup (string popupText, string popupTitle, PopupType type, Action afterMethod = null) {
        Debug.Log(popupTitle);
        StartCoroutine(StartPopup(popupText,popupTitle,type,afterMethod));
    }

    public IEnumerator StartPopup (string popupText, string popupTitle, PopupType type, Action afterMethod = null) {
        if (popupActivated) yield break;
        afterCoroutineMethod = afterMethod;
        popupWindow.SetActive(true);
        if (lvlUpButtons != null) lvlUpButtons.SetActive(false);
        if (dropWeaponButtons != null) dropWeaponButtons.SetActive(false);
        infoButton.SetActive(false);
        switch (type) {
            case PopupType.Information :
                infoButton.SetActive(true);
                break;
            case PopupType.LevelUp :
                if (lvlUpButtons != null) lvlUpButtons.SetActive(true);
                break;
            case PopupType.DropWeapon :
                if (dropWeaponButtons != null) dropWeaponButtons.SetActive(true);
                break;
        }
        txt.text = popupText;
        title.text = popupTitle;
        Time.timeScale = 0;
        popupActivated = true;
        while (popupActivated) yield return null;
    }

    public void UnactivatePopup () {
        popupActivated = false;
        popupWindow.SetActive(false);
        if (afterCoroutineMethod != null) afterCoroutineMethod();
    }

    public void PrepareDropWeapon (WeaponData pickedUpWeapon) {
        dropWeaponButtons.GetComponent<PopupDropWeapon>().AssignWeaponToButton(pickedUpWeapon);
    }
}
