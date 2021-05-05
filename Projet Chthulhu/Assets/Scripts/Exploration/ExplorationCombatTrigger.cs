using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplorationCombatTrigger : MonoBehaviour
{
    public string s;
    private Ennemy ennemy;
    //private ennemis Ennemis;
    private NavMeshAgent theAgentEnnemy;
    private PlayerDeplacement player;


    // Start is called before the first frame update
    void Start()
    {
        ennemy = GetComponentInParent<Ennemy>();
        theAgentEnnemy = ennemy.GetComponent<NavMeshAgent>();
        player = ennemy.player;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            ennemy.hasDetectedPlayer = true;
            theAgentEnnemy.SetDestination(this.transform.position);
            ActorEntity actorPriority = player.GetComponent<ActorEntity>();
            if (s == "ennemis" || !player.stealth)
            {
                actorPriority = ennemy.GetComponent<ActorEntity>();
            }
            player.ToggleBattle(actorPriority);
        }
    }
}
