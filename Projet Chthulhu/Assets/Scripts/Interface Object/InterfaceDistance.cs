using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceDistance : MonoBehaviour
{
    [SerializeField] private float maxDist;
    public bool isInteractable = false;
    private OutlineGenerator outlineGenerator;
    // Update is called once per frame
    private void Start() {
        outlineGenerator = GetComponent<OutlineGenerator>();
        if (outlineGenerator == null) Debug.LogError(this + " should have the OutlineGenerator component in order to work");
    }
    void Update()
    {
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
        outlineGenerator.ToggleOutline(value);
        //add shader here
    }
}
