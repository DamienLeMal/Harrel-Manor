using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemis : MonoBehaviour
{
    public GameObject targetDestination;
    NavMeshAgent theAgent;
    //public Collider detectorPlayer;
    private bool hasDetectedPlayer;

    private GameObject player;
    private float countTime;
    public float maxTime = 4f;

    // Start is called before the first frame update



    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
        hasDetectedPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDetectedPlayer) 
        {
            theAgent.SetDestination(targetDestination.transform.position);
        }

        else
        {
            theAgent.SetDestination(player.transform.position);
            countTime += Time.deltaTime;
            if (countTime >= maxTime)
            {
                hasDetectedPlayer = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            hasDetectedPlayer = true;
            countTime = 0;
            Debug.Log("FindPlayer");
        }
    }
}
