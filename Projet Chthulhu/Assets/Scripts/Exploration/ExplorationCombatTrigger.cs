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
    private RaycastHit[] hits;


    // Start is called before the first frame update
    void Start()
    {
        ennemy = GetComponentInParent<Ennemy>();
        theAgentEnnemy = ennemy.GetComponent<NavMeshAgent>();
        player = ennemy.player;
    }
    private void OnTriggerStay(Collider other)
    {
        if (ennemy == null) return;
        if (ennemy.inCombat) return;
        
        LaunchRaycastCheck();
        if (other.tag == "Player" && hits.Length == 0)
        {
            ActorEntity actorPriority = player.GetComponent<ActorEntity>();
            if (s == "ennemis" || !player.stealth)
            {
                actorPriority = ennemy.GetComponent<ActorEntity>();
            }
            ennemy.SetCombatMode();
            player.SetCombatMode(actorPriority, ennemy.GetComponent<EnnemyEntity>());
        }
        
    }

    private void LaunchRaycastCheck()
    {
        Vector3 direction = (ennemy.transform.position - player.transform.position);
        hits = Physics.RaycastAll(player.transform.position, direction, Mathf.Infinity, 14);

        //Debug.Log(hits.Length);

        if (hits.Length != 0)
        {
            Debug.DrawLine(ennemy.transform.position, player.transform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(ennemy.transform.position, player.transform.position, Color.green);
        }
    }
}
