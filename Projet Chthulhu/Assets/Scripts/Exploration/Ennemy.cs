using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    public GameObject targetDestination;
    NavMeshAgent theAgent;
    public bool hasDetectedPlayer;
    private bool playerStealth;

    public SphereCollider bigDetect;
    public SphereCollider lilDetect;
    [HideInInspector] public PlayerDeplacement player;

    void Awake()
    {
        theAgent = GetComponent<NavMeshAgent>();
        hasDetectedPlayer = false;
        GameObject thePlayer = GameObject.Find("player");
        player = thePlayer.GetComponent<PlayerDeplacement>();

    }

    // Update is called once per frame
    void Update()
    {
        playerStealth = player.stealth;
        changeDetection(playerStealth);


        if (!hasDetectedPlayer)
        {
            theAgent.SetDestination(targetDestination.transform.position);
        }

        else if (hasDetectedPlayer)
        {
            //theAgent.SetDestination(this.transform.position);
            theAgent.isStopped = true;
        }
    }


    private void changeDetection(bool state)
    {
        lilDetect.enabled = state;
        bigDetect.enabled = !state;
    }
}
