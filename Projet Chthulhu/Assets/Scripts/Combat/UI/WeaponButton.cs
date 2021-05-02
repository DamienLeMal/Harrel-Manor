using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform attackContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData) {
        foreach (Transform child in attackContainer) {
            if (child.GetComponent<Button>() == null) continue;
            child.gameObject.SetActive(true);
            LeanTween.moveY(attackContainer.gameObject,attackContainer.position.y+20f,0.2f);
            LeanTween.scale(attackContainer.gameObject,Vector3.one*1.2f,0.2f);
            LeanTween.alphaCanvas(attackContainer.GetComponent<CanvasGroup>(),1f,0.2f);
        }
    }

    public void  OnPointerExit(PointerEventData eventData) {
        foreach (Transform child in attackContainer) {
            if (child.GetComponent<Button>() == null) continue;
            Invoke("Unactive",0.2f);
            LeanTween.moveY(attackContainer.gameObject,attackContainer.position.y-20f,0.2f);
            LeanTween.scale(attackContainer.gameObject,Vector3.one,0.2f);
            LeanTween.alphaCanvas(attackContainer.GetComponent<CanvasGroup>(),0f,0.2f);
        }
    }

    private void Unactive () {
        foreach (Transform child in attackContainer) {
            child.gameObject.SetActive(false);
        }
    }
}
