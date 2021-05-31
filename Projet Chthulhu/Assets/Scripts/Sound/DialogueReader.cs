using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueReader : MonoBehaviour
{
    private ActorDialogue dialogue;

    private void Start() {
        SoundEventManager.current.onDialogue += StartDialogueCoroutine;
        dialogue = GetComponent<PlayerEntity>().GetDialogue();
    }

    private void StartDialogueCoroutine (int id) {
        StartCoroutine(ReadDialogue(id));
    }

    IEnumerator ReadDialogue (int id) {
        yield return new WaitForSeconds(0.1f);
    }
}
