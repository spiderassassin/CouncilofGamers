using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;
using FirstGearGames.SmoothCameraShaker;
using System.Threading;
using UnityEngine.Experimental.GlobalIllumination;


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

    //public GameObject prompt;
    //private TextMeshProUGUI promptText;
    public GameObject wave;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI baseHealthText;

    private float timer;



    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        playerSpeaking = new Queue<bool>();
        waveText = wave.GetComponent<TextMeshProUGUI>();
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger=null, bool advanceGameStage=false)
    {
        currentDialogue = dialogue;
        Debug.Log("current dialogue: " + currentDialogue + " " + currentDialogue.name);
        currentDialogue.dialogueOver = false;
        currentDialogueTrigger = dialogueTrigger;
        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.canStartDialogue = false;
            currentDialogueTrigger.conversationStartPrompt.SetActive(false);
            Debug.Log("setting prompt to false");
        }
        
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
        //dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);
        //dialogueText.text = sentence;
        



        timer = 0;
        int i = 0;
        dialogueText.text = "";

        while (i < sentence.Length)
        {
            //Debug.Log(timer);
            timer += Time.deltaTime;
            if (timer > 0.02f)
            { 
                dialogueText.text += sentence[i];
                if (i < sentence.Length - 1)
                {
                    dialogueText.text += sentence[i + 1];
                    i++;
                }

                if (i < sentence.Length - 2)
                {
                    dialogueText.text += sentence[i + 1];
                    i++;
                }
                
                //Debug.Log(dialogueText.text);
                i++;
                timer = 0;
            }

            else if (timer > 0.0067f)
            {
                dialogueText.text += sentence[i];
                i++;
                timer = 0;
            }
            yield return null;
        }

        SoundManager.Instance.hhhh.setParameterByName("isTalking", 1);
        GameManager.Instance.nextSentenceReady = true;
        yield return null;

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
        if(sentence == "I'll watch you die. I'll savour every moment while you're torn apart. And I'll bring your skull back to Hell myself.")
        {
            NPCSprite.GetComponent<paroleAnimations>().betray();

        }

        if (sentence == "Don't you get it?")
        {
            SoundManager.Instance.betrayal.start();
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

        if (sentence == "… Well on the plus side, I also sold fireworks.")
        {

            NPCSprite.GetComponent<paroleAnimations>().notinquisitive();

        }
        if (sentence == "*coo* *coo*")
        {
            print("cococo");
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.pigeons, player.transform.position);

        }

        if (sentence.Contains("Bloodrush"))
        {
            WaveManager.Instance.bloodrushBar.SetActive(true);
            StartCoroutine(WaveManager.Instance.bloodrushBar.GetComponent<UIElement>().SizeFlash(1.2f));
            //Debug.Log("bloodrush bar activated");
        }

        if (sentence.Contains("Over here, near the exit"))
        {
            //Debug.Log("correct line");
            WaveManager.Instance.triggerAreaForParoleDialogue.GetComponent<DialogueTrigger>().conversationStartPrompt.SetActive(false);
        }

        if (sentence.Contains("watch its health carefully")){
            StartCoroutine(baseHealthText.GetComponent<UIElement>().SizeFlash(1.2f));
        }

        string[] rumblingSentences = {
            "Nevermind, time's up. I feel the next wave approaching.",
            "In a pinch you might be able to salvage some extra demon blood in the skulls lying around the place. Just smash'em for more fuel.",
            "I'll watch you die. I'll savour every moment while you're torn apart. And I'll bring your skull back to Hell myself.",
        };

        if (rumblingSentences.Contains(sentence)) {
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.rumbling, NPCSprite.transform.position);
            CameraShakerHandler.Shake(GameManager.Instance.rumblingShake);
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

        if (GameManager.Instance.gameStage == GameManager.GameStage.Downtime1)
        {
            // Show prompt telling player to follow the voice.
            wave.SetActive(true);
            waveText.text = "Find the source of the strange voice";
        }

        if (GameManager.Instance.gameStage == GameManager.GameStage.Ending)
        {
            // Set up wave manager so the ending wave gets triggered.
            WaveManager.Instance.wavemode = true;
            WaveManager.Instance.isSpawning = false;
        }
    }

    
}
