using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectObject();
        }



    }

    private void DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if(Physics.Raycast(ray, out hit))
        {

            if(hit.collider.gameObject.GetComponent<IClicked>() != null)
            {
                hit.collider.gameObject.GetComponent<IClicked>().onClickAction();
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
