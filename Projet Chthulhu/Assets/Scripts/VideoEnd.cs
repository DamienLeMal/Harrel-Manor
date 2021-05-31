using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoEnd : MonoBehaviour
{
    [SerializeField] GameObject targetVideo;
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
            targetVideo.SetActive(false);
        }
        else
        {
            timea += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timea = timemax + 0.1f;
        }

    }
    
}
