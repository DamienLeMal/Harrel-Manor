using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager current;
    public float maxDistInteract;
    private void Awake() {
        current = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused) return;
        if (Input.GetMouseButtonDown(0))
        {
            DetectObject();
        }
    }

    private void DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if(hit.collider.gameObject.GetComponent<IClicked>() != null && !CombatManager.current.combatOn)
            {
                hit.collider.gameObject.GetComponent<IClicked>().OnClickAction();
            }
        }
    }
}
