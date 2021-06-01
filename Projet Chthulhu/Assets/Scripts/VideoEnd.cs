using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoEnd : MonoBehaviour
{
    [SerializeField] private float timemax;
    [SerializeField] private float timea;

    // Start is called before the first frame update
    void Start()
    {
       
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

        /*

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timea = timemax + 0.1f;
        }

        */
    }
    
}
