using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deplacement : MonoBehaviour
{

    private bool asArrived = true;
    public GameObject targetPrefab;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)&& asArrived == true) 
        {
            //asArrived = false;
            targetPos = new Vector3(Input.mousePosition.x,0,Input.mousePosition.y);
            Instantiate(targetPrefab, targetPos, transform.rotation);
        }
    }

    
    
}
