using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDeplacement : MonoBehaviour
{
    NavMeshAgent agent;

    public float rotateSpeedMovement = 0.1f;
    float rotateVelocity;

    public bool stealth = false;
    public bool inBattle;

    public float minSpeed = 5;
    public float maxSpeed = 10;


    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inBattle)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    agent.SetDestination(hit.point);

                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
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

        
    }

    
    public void SetCombatMode(ActorEntity actorPriority)
    {
        Debug.Log("enbattle");
        agent.SetDestination(this.transform.position);
        agent.isStopped = true;
        agent.enabled = false;
        inBattle = true;
        GetComponent<PlayerEntity>().manager.StartCombat(actorPriority);
        GetComponent<Rigidbody>().detectCollisions = false;
    }

    public void SetExplorationMode()
    {
        Debug.Log("enexplo");
        agent.enabled = true;
        agent.isStopped = false;
        inBattle = false;
        GetComponent<Rigidbody>().detectCollisions = true;
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
