using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool dialogueStarted;


    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggering dialogue");
        if (!dialogueStarted)
        {
            TriggerDialogue();
            dialogueStarted = true;
        }
    }
}
