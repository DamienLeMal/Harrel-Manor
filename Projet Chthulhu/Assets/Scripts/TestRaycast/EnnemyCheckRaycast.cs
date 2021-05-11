using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyCheckRaycast : MonoBehaviour
{
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = (player.transform.position - transform.position);///////////////////////////////
        Ray raycastToPlayer = new Ray(transform.position, player.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 1000f);

        Debug.Log(hits.Length);
        if (hits.Length != 0)
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.green);
            //Debug.Log("pas mur");
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.gameObject.name);
            }
        }
        else
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.red);
            //Debug.Log("mur");
        }
    }
}
