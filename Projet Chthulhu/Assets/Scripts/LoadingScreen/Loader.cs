using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField]private string sceneName;
    private bool loadScene = false;
   
    [Header("Slider de chargement")]
    public Slider sliderBar;
    


    // Start is called before the first frame update
    void Start()
    {

        sliderBar.maxValue = 1f;

        sceneName = PlayerPrefs.GetString("targetscene"); // prend en valeur la scène a chargé


        Invoke("StartLoading", 1F);


        //sliderBar.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // If the player has pressed the space bar and a new scene is not loading yet...
        if (/*Input.GetKeyUp(KeyCode.Space) &&*/ loadScene == false)
        {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            //Visible Slider Progress bar
            sliderBar.gameObject.SetActive(true);


            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene(sceneName));

        }

    }

    void StartLoading()
    {
        // ...set the loadScene boolean to true to prevent loading a new scene more than once...
        loadScene = true;

        //Visible Slider Progress bar
        sliderBar.gameObject.SetActive(true);


        // ...and start a coroutine that will load the desired scene.
        StartCoroutine(LoadNewScene(sceneName));
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene(string sceneName)
    {

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            sliderBar.value = progress;
            Debug.Log(progress);
            yield return null;

        }

    }


}
