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
    [SerializeField] private Text title;
    [SerializeField] private Text txt;
    [SerializeField] private GameObject infoButton;
    [SerializeField] private GameObject lvlUpButtons;
    private bool popupActivated = false;
    private Action afterCoroutineMethod;
    

    private void Start() {
        //ActivatePopup("text", PopupType.Information);
    }

    public void ActivatePopup (string popupText, string popupTitle, PopupType type, Action afterMethod = null) {
        StartCoroutine(StartPopup(popupText,popupTitle,type,afterMethod));
    }

    public IEnumerator StartPopup (string popupText, string popupTitle, PopupType type, Action afterMethod = null) {
        if (popupActivated) yield break;
        Debug.Log("call popup");
        afterCoroutineMethod = afterMethod;
        popupWindow.SetActive(true);
        switch (type) {
            case PopupType.Information :
                if (lvlUpButtons != null) lvlUpButtons.SetActive(false);
                infoButton.SetActive(true);
                break;
            case PopupType.LevelUp :
                if (lvlUpButtons != null) lvlUpButtons.SetActive(true);
                infoButton.SetActive(false);
                break;
        }
        txt.text = popupText;
        title.text = popupText;
        Time.timeScale = 0;
        popupActivated = true;
        while (popupActivated) yield return null;
    }

    public void UnactivatePopup () {
        popupActivated = false;
        popupWindow.SetActive(false);
        if (afterCoroutineMethod != null) afterCoroutineMethod();
        Debug.Log("unactivation");
    }
}
