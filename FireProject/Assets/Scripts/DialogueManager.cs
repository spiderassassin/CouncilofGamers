using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


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
    private bool advanceGameStageOnEnd;
    public AudioClip dialoguesound;

    public string sentence;

    private Dialogue currentDialogue;

    

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        playerSpeaking = new Queue<bool>();
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger=null, bool advanceGameStage=false)
    {
        currentDialogue = dialogue;
        Debug.Log("current dialogue: " + currentDialogue + " " + currentDialogue.name);
        currentDialogue.dialogueOver = false;
        currentDialogueTrigger = dialogueTrigger;
        nameText.text = dialogue.name;
        animator.SetBool("isOpen", true);
        actionPromptsHUD.SetActive(false);
        GameManager.Instance.dialogueState = true;
        advanceGameStageOnEnd = advanceGameStage;

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
        //Debug.Log("starting planning");
        
        DisplayNextSentence();



    }

    public IEnumerator ShowOneLetterAtATime(string sentence)
    {
        //dialogueText.text = sentence;
        dialogueText.text = "";
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogueText.text += sentence[i];
            yield return new WaitForSeconds(0.001f);
        }

        GameManager.Instance.nextSentenceReady = true;
    }

    public void DisplayNextSentence()
    {
        GameManager.Instance.nextSentenceReady = false;
        if (sentences.Count == 0)
        {
            
            EndDialogue();
            return ;
        }
        sentence = sentences.Dequeue();
        
        bool speakingBool = playerSpeaking.Dequeue();
        if (currentDialogueTrigger != null)
            SoundManager.Instance.PlaySoundOnce(dialoguesound, transform);
        {
            if (speakingBool)
            {
                //currentDialogueTrigger.dialogue.name = "You";
                nameText.text = "You";
            }
            else
            {
                nameText.text = "Parole Guard";
                //currentDialogueTrigger.dialogue.name = "ParoleGuard";
            }
        }

        StartCoroutine(ShowOneLetterAtATime(sentence));
        
    }

    public void EndDialogue()
    {
        currentDialogue.dialogueOver = true;
        animator.SetBool("isOpen", false);
        actionPromptsHUD.SetActive(true);
        GameManager.Instance.dialogueState = false;
        if (advanceGameStageOnEnd) {
            // Set next game stage.
            GameManager.Instance.gameStage++;
            // Hide the parole guard sprite.
            WaveManager.Instance.paroleGuardSprite.SetActive(false);
        }
    }

    
}
