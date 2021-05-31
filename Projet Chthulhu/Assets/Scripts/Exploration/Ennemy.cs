using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    public GameObject targetDestination;
    NavMeshAgent theAgent;
    public bool inCombat = false;

    public SphereCollider sphereDetec;
    [SerializeField] private MeshCollider viewDetect;
    [HideInInspector] public PlayerDeplacement player;

    [SerializeField] private int minSize = 1, maxSize = 2;

    void Awake()
    {
        theAgent = GetComponent<NavMeshAgent>();
        GameObject thePlayer = GameObject.Find("Player");
        player = thePlayer.GetComponent<PlayerDeplacement>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!inCombat)
        {
            changeDetection(player.stealth);
        }
    }

    public void SetCombatMode ()
    {
        theAgent.SetDestination(this.transform.position);
        theAgent.isStopped = true;
        Destroy(sphereDetec);
        Destroy(viewDetect);
        Destroy(theAgent);
        inCombat = true;
    }


    private void changeDetection(bool state)
    {

        if (state)
        {
            sphereDetec.radius = minSize;
        }
        else
        {
            sphereDetec.radius = maxSize;
        }
        /*
        lilDetect.enabled = state;
        bigDetect.enabled = !state;
        */
    }
}
