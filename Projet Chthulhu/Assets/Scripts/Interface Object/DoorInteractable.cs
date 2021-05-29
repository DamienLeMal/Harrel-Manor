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

    private void Start() {
        collision = GetComponent<NavMeshObstacle>();
    }
    public void OnClickAction () {
        if (isLocked) {
            if (!CombatManager.current.player.HasKey(keyId)) return;
            //Unlock sound
            isLocked = false;
        }
        //OpenDoor
        GetComponent<Animator>().SetTrigger("DoorAction");

        //Temp
        ToggleDoor();
    }

    /// <summary>
    /// Toggle Open or Close Door, meant to be activated on animation end.
    /// </summary>
    private void ToggleDoor () {
        //CombatManager.current.grid[coordX,coordY].DoorToggleOpenClose();
        collision.enabled = !collision.enabled;

    }
}
