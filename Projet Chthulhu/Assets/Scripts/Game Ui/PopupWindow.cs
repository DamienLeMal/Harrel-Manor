using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    

    private void Start() {
        ActivatePopup("text", PopupType.Information);
    }

    public void ActivatePopup (string popupText, PopupType type) {
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
    }

    public void UnactivatePopup () {
        Debug.Log("unactivation");
    }
}
