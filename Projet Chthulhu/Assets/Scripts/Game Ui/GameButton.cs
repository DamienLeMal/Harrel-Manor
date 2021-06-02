using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public void TemporaryUnactivate (Button button = null, float duration = 0f) {
        //Unactivate button
        if (button != null) {
            button.interactable = false;
        }
        if (duration == 0f) return;
        StartCoroutine(Reactivate(button,duration));
    }
    public void TemporaryUnactivate (Button[] buttons = null, float duration = 0f) {
        //Unactivate button
        if (buttons != null) {
            foreach (Button b in buttons) {
                b.interactable = false;
            }
        }
        if (duration == 0f) return;
        StartCoroutine(Reactivate(buttons,duration));
    }

    IEnumerator Reactivate (Button buttons, float duration) {
        yield return new WaitForSeconds (duration);
        buttons.interactable = true;
    }
    IEnumerator Reactivate (Button[] buttons, float duration) {
        yield return new WaitForSeconds (duration);
        foreach (Button b in buttons) {
            b.interactable = true;
        }
    }
    public void ReactivateButton (Button buttons) {
        buttons.interactable = true;
    }
    public void ReactivateButton (Button[] buttons) {
        foreach (Button b in buttons) {
            b.interactable = true;
        }
    }
}
