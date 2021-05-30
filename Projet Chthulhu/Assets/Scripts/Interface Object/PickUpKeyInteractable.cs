using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKeyInteractable : MonoBehaviour, IClicked
{
    [SerializeField] private int keyId;
    public void OnClickAction () {
        CombatManager.current.player.AddKey(keyId);
        Destroy(gameObject);
    }
}
