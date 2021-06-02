using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorInteractable : MonoBehaviour
{
    private TileEntity doorTile;
    private NavMeshObstacle collision;
    [SerializeField] private bool isLocked;
    [SerializeField] private int keyId;
    private AudioSource source;
    [SerializeField] private AudioClip doorOpen, doorClose;
    private InterfaceDistance interfaceDistance;
    [SerializeField] private Animator animator;
    private bool cooldown;

    private void Start() {
        interfaceDistance = GetComponent<InterfaceDistance>();
        collision = GetComponent<NavMeshObstacle>();
        source = GetComponent<AudioSource>();
    }

    private void GetCorrespondingTile () {
        float minDist = Mathf.Infinity;
        float a;
        TileEntity closestTile = null;
        foreach (TileEntity t in CombatManager.current.grid) {
            if (t == null) { continue; }
            a = (transform.position - t.transform.position).magnitude;
            if (a < minDist) {
                minDist = a;
                closestTile = t;
            }
        }
        doorTile = closestTile;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Player") return;
        TriggerDoor(true);
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag != "Player") return;
        TriggerDoor(false);
    }

    private void TriggerDoor (bool open) {
        if (isLocked && !open) return;
        if (!isLocked && open) return;
        if (doorTile == null) GetCorrespondingTile();
        if (CombatManager.current.combatOn) return;
        if (cooldown) return;
        if (!interfaceDistance.isInteractable) return;
        if (isLocked) {
            if (!CombatManager.current.player.HasKey(keyId)) {
                //locked sound
                return;
            } 
            //Unlock sound
            isLocked = false;
        }
        //OpenDoor
        if (animator.GetAnimatorTransitionInfo(0).duration > 0) {
            return;
        }
        animator.SetTrigger("DoorAction");

        //Temp
        ToggleDoor();
        StartCoroutine(StartCooldown());
    }

    /// <summary>
    /// Toggle Open or Close Door, meant to be activated on animation end.
    /// </summary>
    private void ToggleDoor () {
        //CombatManager.current.grid[coordX,coordY].DoorToggleOpenClose();
        
        collision.enabled = !collision.enabled;
        if (collision.enabled) {
            source.PlayOneShot(doorClose);
            doorTile.tileState = TileState.Block;
        }else{
            source.PlayOneShot(doorOpen);
            doorTile.tileState = TileState.Walk;
        }
    }

    IEnumerator StartCooldown () {
        cooldown = true;
        yield return new WaitForSeconds(0.8f);
        cooldown = false;
    }
}
