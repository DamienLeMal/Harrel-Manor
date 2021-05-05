using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatTrigger : MonoBehaviour
{
    public string s;
    public Ennemis ennemis;
    //private ennemis Ennemis;
    private NavMeshAgent theAgentEnnemis;
    private PlayerDeplacement player;


    // Start is called before the first frame update
    void Start()
    {
        theAgentEnnemis = ennemis.GetComponent<NavMeshAgent>();
        player = ennemis.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            ennemis.hasDetectedPlayer = true;
            theAgentEnnemis.SetDestination(this.transform.position);
            player.ToggleBattle();
            //Debug.Log(s);

            if(s == "ennemis" && player.stealth)
            {
                Debug.Log("Avatage joueur ici");
            }

        }
    }

}
