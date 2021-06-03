using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerDeplacement : MonoBehaviour
{
    NavMeshAgent agent;
    //Animator
    [SerializeField] private Animator m_Animator;

    public float rotateSpeedMovement = 0.1f;
    float rotateVelocity;

    private bool _stealth = false;
    public bool stealth {
        get {return _stealth;}
        set {
            _stealth = value;
            SoundEventManager.current.ActorStealth(_stealth);
        }
    }
    public bool inBattle;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inBattle) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButton(0)) {
            m_Animator.SetBool("isWalking", true);
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
            Vector3 direction = new Vector3(pos.x,0,pos.y);
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(-direction) * Camera.main.transform.parent.transform.rotation,Time.deltaTime * 5f);
            float speed = 100;
            if (_stealth) speed = 50;
            GetComponent<Rigidbody>().AddForce(transform.forward * speed,ForceMode.Force);
            

        }
        if (Input.GetMouseButtonUp(0) || rb.velocity.magnitude < 0.1f) {
            //arete de marcher
            m_Animator.SetBool("isWalking", false);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) //test KeyDown
        {
            ToggleSteath();
        }
    }

    
    public void SetCombatMode(ActorEntity actorPriority, EnnemyEntity firstEnnemy)
    {
        ToggleSteath(false);
        SoundEventManager.current.GamemodeChange();
        agent.SetDestination(this.transform.position);
        agent.isStopped = true;
        agent.enabled = false;
        inBattle = true;
        CombatManager.current.StartCombat(actorPriority,firstEnnemy);
        rb.detectCollisions = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePosition;
    }

    public void SetExplorationMode()
    {
        Debug.Log("enexplo");
        agent.enabled = true;
        agent.isStopped = false;
        inBattle = false;
        rb.detectCollisions = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void ToggleSteath()
    {
        stealth = !stealth;
        if (stealth) m_Animator.speed = 0.5f;
        if (!stealth) m_Animator.speed = 1f;
    }
    private void ToggleSteath(bool state)
    {
        stealth = state;
        if (stealth) m_Animator.speed = 0.5f;
        if (!stealth) m_Animator.speed = 1f;
    }
    
}
