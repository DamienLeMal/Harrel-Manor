using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoEnd : MonoBehaviour
{
    [SerializeField] private float timemax;
    [SerializeField] private float timea;

    [SerializeField] private GameObject skipText;
    [SerializeField] private bool inTime;
    [SerializeField] private float timerSkip;

    // Start is called before the first frame update
    void Start()
    {
        inTime = false;
        skipText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timea >= timemax)
        {
            SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
        }
        else
        {
            timea += Time.deltaTime;
        }


        if (inTime)
        {
            timerSkip += Time.deltaTime;
        }
        
        if (inTime && timerSkip > 3)
        {
            timerSkip = 0;
            skipText.SetActive(false);
            inTime = !inTime;
        }
        
            
        
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inTime)
            {
                SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
            }
            else
            {
                inTime = true;
                skipText.SetActive(true);
            }
        }

        
    }
    
}
