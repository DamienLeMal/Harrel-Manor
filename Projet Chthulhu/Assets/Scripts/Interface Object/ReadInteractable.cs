using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadInteractable : MonoBehaviour, IClicked
{
    [SerializeField] [TextArea] private string title, content;
    private InterfaceDistance interfaceDistance;

    private void Awake() {
        interfaceDistance = GetComponent<InterfaceDistance>();
        if (interfaceDistance == null) Debug.LogError(this + " should have the InterfaceDistance component in order to work");
    }

    public void OnClickAction () {
        if (!interfaceDistance.isInteractable) return;
        Debug.Log("interacted");
        InteractionManager.current.windowRead.ActivatePopup(content,title,PopupType.Information);
    }
}
