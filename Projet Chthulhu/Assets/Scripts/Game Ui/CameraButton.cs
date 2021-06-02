using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : GameButton
{   
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private Button[] rotateButtons = new Button[2];
    public void RotateCamera (bool clockwise) {
        float rotation = cameraTarget.transform.eulerAngles.y;
        TemporaryUnactivate(rotateButtons,0.8f);
        if (clockwise) {
            LeanTween.rotateY(cameraTarget,rotation+90f,0.8f).setEaseInOutQuint();
        }else{
            LeanTween.rotateY(cameraTarget,rotation-90f,0.8f).setEaseInOutQuint();
        }
    }
}
