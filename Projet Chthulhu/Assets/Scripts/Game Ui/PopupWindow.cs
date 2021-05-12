using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PopupType {
    Information,
    LevelUp
}
public class PopupWindow : MonoBehaviour
{
    [SerializeField] private GameObject popupWindow;
    [SerializeField] private Text txt;
    [SerializeField] private GameObject infoButton;
    [SerializeField] private GameObject lvlUpButtons;
    private bool popupActivated = false;
    private Action afterCoroutineMethod;
    

    private void Start() {
        //ActivatePopup("text", PopupType.Information);
    }



    public IEnumerator ActivatePopup (string popupText, PopupType type, Action afterMethod = null) {
        Debug.Log("call popup");
        afterCoroutineMethod = afterMethod;
        popupWindow.SetActive(true);
        switch (type) {
            case PopupType.Information :
                lvlUpButtons.SetActive(false);
                infoButton.SetActive(true);
                break;
            case PopupType.LevelUp :
                lvlUpButtons.SetActive(true);
                infoButton.SetActive(false);
                break;
        }
        txt.text = popupText;
        Time.timeScale = 0;
        popupActivated = true;
        while (popupActivated) yield return null;
    }

    public void UnactivatePopup () {
        popupActivated = false;
        popupWindow.SetActive(false);
        afterCoroutineMethod();
        Debug.Log("unactivation");
    }
}
