using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActorUi : MonoBehaviour
{
    [SerializeField] private Text damageText = null;
    private Camera cam;

    private void Awake() {
        cam = FindObjectOfType<Camera>();
    }

    public void ShowDamageAmount (int damageAmount) {
        if (damageAmount == 0) {
            SetDamageText("Raté!");
        }else{
            SetDamageText(damageAmount.ToString());
        }
        AnimateDamageText();
    }

    private void SetDamageText (string txt) {
        damageText.text = txt;
        damageText.color = new Color(255f,255f,255f,1f);
        damageText.transform.localPosition = Vector3.zero;
        damageText.transform.LookAt(2*transform.position - cam.transform.position);
    }

    private void AnimateDamageText () {
        LeanTween.moveLocalY(damageText.gameObject,1f,0.5f);
        LeanTween.textAlpha(damageText.rectTransform,0,0.5f);
    }
}
