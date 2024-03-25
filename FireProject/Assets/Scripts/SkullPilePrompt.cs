using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPilePrompt : MonoBehaviour
{

    public Dialogue skullPromptDialogue;
    public bool dialogueStarted;
    public bool canStartDialogue;


    public void OnTriggerEnter(Collider other)
    {
        canStartDialogue = true;
        
    }

    public void Update()
    {
        if (canStartDialogue && !dialogueStarted)
        {
            FindObjectOfType<DialogueManager>().StopAllCoroutines();
            FindObjectOfType<DialogueManager>().StartDialogue(skullPromptDialogue);
            dialogueStarted = true;
            this.gameObject.SetActive(false);
        }
    }

}
