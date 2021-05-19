using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private int id;
    private void OnTriggerEnter(Collider other) {
        SoundEventManager.current.Dialogue(id);
        Destroy(gameObject);
    }
}
