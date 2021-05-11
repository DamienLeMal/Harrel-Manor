using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupButton : MonoBehaviour
{
    [SerializeField] PopupWindow popupWindow = null;
    virtual public void ButtonAction () {
        Time.timeScale = 1;
        popupWindow.UnactivatePopup();
    }
}
