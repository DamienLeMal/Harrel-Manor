using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemis : MonoBehaviour
{
    public GameObject targetDestination;
    NavMeshAgent theAgent;
    private bool hasDetectedPlayer;
    private bool playerStealth;

    public Collider bigDetect;
    public Collider lilDetect;
    private GameObject thePlayer;
    deplacement stealth;


    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
        hasDetectedPlayer = false;
        GameObject thePlayer = GameObject.Find("player");
        stealth = thePlayer.GetComponent<deplacement>();

    }

    // Update is called once per frame
    void Update()
    {
        playerStealth = stealth.stealth;
        changeDetection(playerStealth);


        if (!hasDetectedPlayer)
        {  
            theAgent.SetDestination(targetDestination.transform.position); 
        }

        else if (hasDetectedPlayer)
        {
            theAgent.SetDestination(this.transform.position);
        }
       

        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            hasDetectedPlayer = true;           
            Debug.Log("Combat !");
        }
    }

    private void changeDetection(bool state)
    {
            lilDetect.enabled = state;
            bigDetect.enabled = !state;
    }
}
