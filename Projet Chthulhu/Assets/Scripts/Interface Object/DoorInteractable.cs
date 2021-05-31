using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IClicked
{
    [SerializeField] private int coordX, coordY;
    public void OnClickAction () {
        //OpenDoor

        //Temp
        ToggleDoor();
    }

    /// <summary>
    /// Toggle Open or Close Door, meant to be activated on animation end.
    /// </summary>
    private void ToggleDoor () {
        CombatManager.current.grid[coordX,coordY].DoorToggleOpenClose();
    }
}
