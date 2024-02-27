using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool dialogueStarted;
    public bool canStartDialogue;
    public GameObject conversationStartPrompt;


    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void OnTriggerEnter(Collider other)
    {
        /*
        Debug.Log("triggering dialogue");
        if (!dialogueStarted)
        {
            TriggerDialogue();
            dialogueStarted = true;
        }*/

        conversationStartPrompt.SetActive(true);
        canStartDialogue = true;
    }

    public void OnTriggerExit(Collider other)
    {
        conversationStartPrompt.SetActive(false);
    }

    
}
