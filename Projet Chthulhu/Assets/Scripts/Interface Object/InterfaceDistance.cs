using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceDistance : MonoBehaviour
{
    private float maxDist;
    [HideInInspector] public bool isInteractable = false;
    private OutlineGenerator outlineGenerator;
    // Update is called once per frame
    private void Start() {
        outlineGenerator = GetComponent<OutlineGenerator>();
        maxDist = InteractionManager.current.maxDistInteract;
        if (Vector3.Distance(CombatManager.current.player.transform.position,transform.position) > maxDist) {
            MakeInteractable(false);
        }else{
            MakeInteractable(true);
        }
    }
    void Update()
    {
        if (CombatManager.current.combatOn) {
            if (isInteractable == true) MakeInteractable(false);
            return;
        }
        if (Vector3.Distance(CombatManager.current.player.transform.position,transform.position) > maxDist && isInteractable) {
            MakeInteractable(false);
            return;
        }
        if (Vector3.Distance(CombatManager.current.player.transform.position,transform.position) <= maxDist && !isInteractable) {
            MakeInteractable(true);
            return;
        }
    }

    private void MakeInteractable (bool value) {
        isInteractable = value;
        if (outlineGenerator != null) outlineGenerator.ToggleOutline(value);
        //add shader here
    }
}
