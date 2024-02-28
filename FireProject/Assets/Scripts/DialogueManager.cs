using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject actionPromptsHUD;

    public GameObject NPCSprite;

    public GameObject conversationStartPrompt;

    public Animator animator;

    private Queue<string> sentences;
    private Queue<bool> playerSpeaking;

    private DialogueTrigger currentDialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        playerSpeaking = new Queue<bool>();
        
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger=null)
    {
        currentDialogueTrigger = dialogueTrigger;
        nameText.text = dialogue.name;
        animator.SetBool("isOpen", true);
        actionPromptsHUD.SetActive(false);
        GameManager.Instance.dialogueState = true;
        

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        playerSpeaking.Clear();
        foreach (bool speakingBool in dialogue.playerSpeaking)
        {
            playerSpeaking.Enqueue(speakingBool);
        }

        DisplayNextSentence();



    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        
        bool speakingBool = playerSpeaking.Dequeue();
        if (currentDialogueTrigger != null)
        {
            Debug.Log("current di");
            if (speakingBool)
            {
                //currentDialogueTrigger.dialogue.name = "You";
                nameText.text = "You";
            }
            else
            {
                nameText.text = "Parole Officer";
                //currentDialogueTrigger.dialogue.name = "ParoleGuard";
            }
        }
        
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        actionPromptsHUD.SetActive(false);
        GameManager.Instance.dialogueState = false;
        
    }

    
}
