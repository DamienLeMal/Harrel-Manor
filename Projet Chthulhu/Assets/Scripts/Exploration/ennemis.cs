using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemis : MonoBehaviour
{
    public GameObject targetDestination;
    NavMeshAgent theAgent;

    // Start is called before the first frame update



    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        theAgent.SetDestination(targetDestination.transform.position);
    }
}
