using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEditor.Experimental.GraphView;


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

    public GameObject dialogueBox;
    public GameObject player;
    public int hhhcounter;



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
        SoundManager.Instance.hhhh.setParameterByName("isTalking", 1);



        GameManager.Instance.nextSentenceReady = true;
    }

    public void DisplayNextSentence()
    {
        
        if (sentences.Count == 0)
        {
            
            EndDialogue();
            return ;
        }
        sentence = sentences.Dequeue();
        print(sentence);
        if(sentence == "At this point, I wish I had gotten Genderen sent down here instead of you. He would've died easily.")
        {
            SoundManager.Instance.betrayal.start();
            NPCSprite.GetComponent<paroleAnimations>().betray();

        }

        if (sentence == "By the way, I could tell you were holding back out there. ")
        {
            
            player.GetComponent<Controller>().snapAllowed = true;
            player.GetComponent<Controller>().flameAttackAllowed = true;

        }
        if (sentence == "Y’know I heard about some of the stuff that got you locked down here. Were you <i>selling</i> your <i>blood</i>?")
        {

            NPCSprite.GetComponent<paroleAnimations>().inquisitive();

        }

        if (sentence == "…Well on the plus side, I also sold fireworks.")
        {

            NPCSprite.GetComponent<paroleAnimations>().notinquisitive();

        }
        if (sentence == "*coo* *coo*")
        {
            print("cococo");
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.pigeons, player.transform.position);

        }

        








        bool speakingBool = playerSpeaking.Dequeue();
        if (currentDialogueTrigger != null)
            
        {
            if (speakingBool)
            {
                //currentDialogueTrigger.dialogue.name = "You";
                nameText.text = "You";
                dialogueBox.GetComponent<Image>().color = new Color32(0, 0, 0, 141);
            }/*
            else
            {
                nameText.text = "Parole Guard";
                //for the hhhh dialogue soundF
                

                //currentDialogueTrigger.dialogue.name = "ParoleGuard";
            }*/
            else if (currentDialogue.name.Contains("Parole"))
            {
                nameText.text = "Parole Guard";
                
                
                    RuntimeManager.DetachInstanceFromGameObject(SoundManager.Instance.hhhh);
                    SoundManager.Instance.hhhh.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    SoundManager.Instance.hhhh = SoundManager.Instance.CreateInstance(FMODEvents.Instance.hhhh);
                    RuntimeManager.AttachInstanceToGameObject(SoundManager.Instance.hhhh, NPCSprite.transform);
                    
                    SoundManager.Instance.hhhh.start();
                    SoundManager.Instance.hhhh.release();
                
                
                dialogueBox.GetComponent<Image>().color = new Color32(41, 8, 8, 141);
            }
            else if (currentDialogue.name.Contains("?"))
            {
                nameText.text = "???";
                dialogueBox.GetComponent<Image>().color = new Color32(41, 8, 8, 141);
            }
            else
            {
                nameText.text = " ";
                dialogueBox.GetComponent<Image>().color = new Color32(0, 0, 0, 141);
            }
        }
        GameManager.Instance.nextSentenceReady = false;

        StartCoroutine(ShowOneLetterAtATime(sentence));
        
    }

    public void EndDialogue()
    {
        currentDialogue.dialogueOver = true;
        animator.SetBool("isOpen", false);
        actionPromptsHUD.SetActive(true);
        GameManager.Instance.dialogueState = false;
        if (advanceGameStageOnEnd) {
            SoundManager.Instance.DowntimeMusicStop();
            // Set next game stage.
            GameManager.Instance.gameStage++;
            // Hide the parole guard sprite.
            //WaveManager.Instance.paroleGuardSprite.SetActive(false);
            NPCSprite.GetComponent<paroleAnimations>().hide();

        }
    }

    
}
