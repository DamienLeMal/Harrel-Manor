using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] [TextArea] private string baseTitle = "";
    [SerializeField] [TextArea] private string baseText = "Default Text";
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private float offsetX = 0f;

    public void SetToolTipText (string title, string descrition) {
        baseTitle = title;
        baseText = descrition;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        TooltipUi.current.yOffset = offsetY;
        TooltipUi.current.xOffset = offsetX;
        TooltipUi.current.SetText(baseTitle,baseText);
        TooltipUi.current.ToggleTooltip(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(TooltipUi.current.rect);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipUi.current.ToggleTooltip(false);
    }
}
