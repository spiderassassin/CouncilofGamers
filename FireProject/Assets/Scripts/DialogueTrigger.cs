using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue downtime1Dialogue;
    public Dialogue downtime2Dialogue;
    public Dialogue downtime3Dialogue;
    public bool dialogueStarted;
    public bool canStartDialogue;
    public GameObject conversationStartPrompt;
    public AudioSource parole;
    public bool inArea;
    public GameObject waveNumberText;

    
    


    public void TriggerDialogue()
    {
        // Trigger the corresponding dialogue based on the current game stage.
        Dialogue downtimeDialogue = null;
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Downtime1:
                downtimeDialogue = downtime1Dialogue;
                break;
            case GameManager.GameStage.Downtime2:
                downtimeDialogue = downtime2Dialogue;
                break;
            case GameManager.GameStage.Downtime3:
                downtimeDialogue = downtime3Dialogue;
                break;
        }
        // Set flag so that at the end of the dialogue, the game stage will advance.
        FindObjectOfType<DialogueManager>().StartDialogue(downtimeDialogue, this, true);
        SoundManager.Instance.hello.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        WaveManager.Instance.arrow.SetActive(false);
        SoundManager.Instance.DowntimeMusicPlay();

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag != "Player")
        {
            return;
        }

        Debug.Log("on trigger stay");
        /*
        Debug.Log("triggering dialogue");
        if (!dialogueStarted)
        {
            TriggerDialogue();
            dialogueStarted = true;
        }*/
        List<GameManager.GameStage> downtimeStages = new List<GameManager.GameStage> {
            GameManager.GameStage.Downtime1,
            GameManager.GameStage.Downtime2,
            GameManager.GameStage.Downtime3
        };

        //if (!canStartDialogue)
        //{
        //    conversationStartPrompt.SetActive(false);
        //}
        if (downtimeStages.Contains(GameManager.Instance.gameStage) && WaveManager.Instance.tutorialSkullPilePunched)
        {
            Debug.Log("on trigger stay if");
            conversationStartPrompt.SetActive(true);
            canStartDialogue = true;
        }
        
        
    }

    public void OnTriggerExit(Collider other)
    {
        conversationStartPrompt.SetActive(false);
        if (waveNumberText != null)
        {
            waveNumberText.GetComponent<UIElement>().SetOriginalSize();
        }
    }





}
