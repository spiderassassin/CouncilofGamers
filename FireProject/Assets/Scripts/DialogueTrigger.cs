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
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, this);
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
        Debug.Log(GameManager.Instance.gameStage);
        if (GameManager.Instance.gameStage == GameManager.GameStage.PreWave1)
        {
            conversationStartPrompt.SetActive(true);
            canStartDialogue = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        conversationStartPrompt.SetActive(false);
    }

    
}
