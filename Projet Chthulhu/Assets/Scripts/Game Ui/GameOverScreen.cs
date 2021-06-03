using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Text text;


    private void OnEnable() {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvas,1,3f).setEaseOutCirc();
        LeanTween.scale(text.gameObject,Vector3.one*4,5f).setEaseInCirc();
        Invoke("AlphaText",2f);
    }

    private void AlphaText() {
        LeanTween.alphaText(text.rectTransform,0,3f);
        Invoke("EndGame",2f);
    }

    private void EndGame () {
        //Go to title screen
        Debug.Log("Go Back to title screen");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("TitleScreen",LoadSceneMode.Single);

    }
}
