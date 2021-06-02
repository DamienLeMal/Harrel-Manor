using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    NavMeshAgent theAgent;
    public bool inCombat = false;

    public SphereCollider sphereDetec;
    [SerializeField] private MeshCollider viewDetect;
    [HideInInspector] public PlayerDeplacement player;

    [SerializeField] private int minSize = 1, maxSize = 2;

    //ref a l'anamitor
    private Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<ActorEntity>().animator;
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
        
        //active la marche
        //if (!m_Animator.GetBool("isWalking"))
        //{
        //    m_Animator.SetBool("isWalking", true);
        //}
        
        
    }

    public void SetCombatMode ()
    {
        //stop la marche
        m_Animator.SetBool("isWalking", false);
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
    private void OnWillRenderObject() {
        if (CombatManager.current.combatOn) return;
        StopCoroutine("NotInSightTimer");
        StartCoroutine("NotInSightTimer");
        SoundEventManager.current.EnnemyInSight();
    }
    IEnumerator NotInSightTimer () {
        yield return new WaitForSeconds(5f);
        SoundEventManager.current.EnnemyLooseSight();
    }
}
