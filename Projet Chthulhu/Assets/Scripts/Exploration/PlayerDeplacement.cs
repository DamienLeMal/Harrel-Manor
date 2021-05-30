using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerDeplacement : MonoBehaviour
{
    NavMeshAgent agent;

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

    public float minSpeed = 5;
    public float maxSpeed = 10;

    private Rigidbody rb;

    [SerializeField]private float minClick = 2;


    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattle) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                if (hit.collider.gameObject.GetComponent<IClicked>() != null) return;
                    if (Vector3.Distance(transform.position,hit.point) > minClick)
                    {
                        agent.SetDestination(hit.point);
                        Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                        transform.eulerAngles = new Vector3(0, rotationY, 0);
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }

                }
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                agent.SetDestination(this.transform.position);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl)) //test KeyDown
            {
                ToggleSteath();
            }
    }

    
    public void SetCombatMode(ActorEntity actorPriority)
    {
        SoundEventManager.current.GamemodeChange();
        agent.SetDestination(this.transform.position);
        agent.isStopped = true;
        agent.enabled = false;
        inBattle = true;
        CombatManager.current.StartCombat(actorPriority);
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
        if (stealth)
        {
            agent.speed = minSpeed;
        }
        else
        {
            agent.speed = maxSpeed;
        }
    }
    
}
