using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUi : MonoBehaviour
{
    public static TooltipUi current;
    [HideInInspector] public RectTransform rect;
    [SerializeField] public RectTransform canvasRect;
    [SerializeField] public Text title;
    [SerializeField] public Text content;
    [HideInInspector] public float yOffset;
    private CanvasGroup canvasGroup;
    private bool isActive;
    
    private void Awake() {
        current = this;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        ToggleTooltip(false);
    }

    public void SetText (string name, string description) {
        title.text = name;
        content.text = description;
    }

    public void ToggleTooltip (bool toggle) {
        isActive = toggle;
        if (toggle) {
            gameObject.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup,1,0.2f).setEaseOutCirc();
        }
        if (!toggle) {
            LeanTween.alphaCanvas(canvasGroup,0,0.2f).setEaseOutCirc();
            Invoke("SetActive",0.2f);
        } 
        
    }
    private void SetActive () {
        if (isActive) return;
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.position = Input.mousePosition + new Vector3 (0,rect.rect.height/2 + yOffset,0);
    }
}
