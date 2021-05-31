using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorInteractable : MonoBehaviour, IClicked
{
    [SerializeField] private int coordX, coordY;
    private NavMeshObstacle collision;
    [SerializeField] private bool isLocked;
    [SerializeField] private int keyId;
    private AudioSource source;
    [SerializeField] private AudioClip doorOpen, doorClose;
    private InterfaceDistance interfaceDistance;
    private Animator animator;
    private bool cooldown;

    private void Start() {
        animator = GetComponent<Animator>();
        interfaceDistance = GetComponent<InterfaceDistance>();
        collision = GetComponent<NavMeshObstacle>();
        source = GetComponent<AudioSource>();
    }
    public void OnClickAction () {
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
            Debug.Log("Stopped");
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
            source.PlayOneShot(doorOpen);
        }else{
            source.PlayOneShot(doorClose);
        }
    }

    IEnumerator StartCooldown () {
        cooldown = true;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
}
