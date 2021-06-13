using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEvent : MonoBehaviour
{
    public AK.Wwise.Event MyEvent;

    // Start is called before the first frame update
    void Start()
    {
        MyEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
