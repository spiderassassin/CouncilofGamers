using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance; //Singleton

    public bool LockPlayerGameplayInput { get; set; }

    [SerializeField]
    private PlayerInput playerInput;
    public Vector2 mouseSensitivity = Vector2.one;
    public bool toggleSprint = false;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction fireAction;
    private InputAction punchAction;
    private InputAction sprintAction;
    private InputAction snapAction;
    private InputAction fireballAction;
    private InputAction startDialogueAction;
    private InputAction pauseAction;

    public float mouseX = 0;
    public float mouseY = 0;
    public float moveX = 0;
    public float moveY = 0;

    public bool jump = false;
    public bool sprintOn = false;
    public bool sprintOff = false;
    public bool takeDamage = false;
    public bool punch => punchAction.WasPerformedThisFrame()&&!LockPlayerGameplayInput;
    public bool fireball => fireballAction.WasPerformedThisFrame() && !LockPlayerGameplayInput;

    public bool fire = false;
    public bool stopfire = false;//called when user releases input
    public bool snap =>snapAction.WasPerformedThisFrame() && !LockPlayerGameplayInput;
    public bool dialogue => jumpAction.WasPerformedThisFrame();

    public bool startwave = false;//this is for testing only, to start waves

    public GameObject dialogueManager;
    public GameObject dialogueTrigger;

    public GameObject pauseMenu;

    private void Awake()
    {
        sprintOff = true;

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Register inputs.
        moveAction = playerInput.currentActionMap.FindAction("Move");
        moveAction.canceled += MoveAction_canceled;
        moveAction.performed += MoveAction_performed;

        jumpAction = playerInput.currentActionMap.FindAction("Jump");
        jumpAction.performed += JumpAction_performed;
        jumpAction.canceled += JumpAction_canceled;

        lookAction = playerInput.currentActionMap.FindAction("Look");
        lookAction.performed += LookAction_performed;
        lookAction.canceled += LookAction_canceled;

        fireAction = playerInput.currentActionMap.FindAction("Fire");
        fireAction.performed += FireAction_performed;
        fireAction.canceled += FireAction_canceled;

        punchAction = playerInput.currentActionMap.FindAction("Punch");
        snapAction = playerInput.currentActionMap.FindAction("Snap");
        fireballAction = playerInput.currentActionMap.FindAction("Fireball");

        sprintAction = playerInput.currentActionMap.FindAction("Sprint");
        sprintAction.performed += SprintAction_performed;
        sprintAction.canceled += SprintAction_canceled;

        startDialogueAction = playerInput.currentActionMap.FindAction("StartDialogue");
        startDialogueAction.performed += StartDialogueAction_performed;
        startDialogueAction.canceled += StartDialogueAction_canceled;

        pauseAction = playerInput.currentActionMap.FindAction("Pause");
        pauseAction.performed += PauseAction_performed;
        pauseAction.canceled += PauseAction_canceled;


    }

    private void SprintAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        if (!toggleSprint)
        {
            sprintOn = false;
            sprintOff = true;
        }
    }

    private void SprintAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        if (toggleSprint)
        {
            sprintOn = !sprintOn;
            sprintOff = !sprintOn;
        }
        else
        {
            sprintOn = true;
            sprintOff = false;
        }
    }

    private void FireAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        stopfire = true;
        fire = false;
    }

    private void FireAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        stopfire = false;
        fire = true;
    }

    private void MoveAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput)
        {
            return;
        }
        moveX = obj.ReadValue<Vector2>().x;
        moveY = obj.ReadValue<Vector2>().y;
    }
    private void MoveAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        moveX = 0;
        moveY = 0;
    }
    private void LookAction_canceled(InputAction.CallbackContext obj)
    {
        // if (LockPlayerGameplayInput) return;
        mouseX = 0;
        mouseY = 0;
    }
    private void JumpAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        jump = false;
    }
    private void LookAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        mouseX = obj.ReadValue<Vector2>().x*mouseSensitivity.x;
        mouseY = obj.ReadValue<Vector2>().y*mouseSensitivity.y;
    }

    
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput && GameManager.Instance.dialogueState)
        {
            if (GameManager.Instance.nextSentenceReady)
            {
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
                return;
            }

            else if (dialogueManager.GetComponent<DialogueManager>().dialogueText.text != dialogueManager.GetComponent<DialogueManager>().sentence)
            {
                dialogueManager.GetComponent<DialogueManager>().dialogueText.text = dialogueManager.GetComponent<DialogueManager>().sentence;
                dialogueManager.GetComponent<DialogueManager>().StopAllCoroutines();
            }
            else
            {
                dialogueManager.GetComponent<DialogueManager>().dialogueText.text = "";
                dialogueManager.GetComponent<DialogueManager>().sentence = "";
                dialogueManager.GetComponent<DialogueManager>().StopAllCoroutines();
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
            }
            
        }

        jump = obj.ReadValueAsButton();
        //Debug.Log("dialogue " + dialouge);

    }

    private void StartDialogueAction_performed(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
        if (dialogueTrigger.GetComponent<DialogueTrigger>().canStartDialogue)
        {
            dialogueManager.GetComponent<DialogueManager>().conversationStartPrompt.SetActive(false);
            dialogueTrigger.GetComponent<DialogueTrigger>().TriggerDialogue();
            
        }
        


    }

    private void StartDialogueAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
    }

    private void PauseAction_performed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.gamePaused)
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.gamePaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            //LockPlayerGameplayInput = false;
            return;
        }

        pauseMenu.SetActive(true);
        GameManager.Instance.gamePaused = true;
        Time.timeScale = 0f;
        //LockPlayerGameplayInput = true;
        //Debug.Log("lock: " + LockPlayerGameplayInput);
    }
    private void PauseAction_canceled(InputAction.CallbackContext obj)
    {
        if (LockPlayerGameplayInput) return;
    }

    void Start()
    {
         
    }

    
    void Update()
    {
        startwave = Input.GetKeyDown(KeyCode.M)&&!LockPlayerGameplayInput;
        if (GameManager.Instance.dialogueState || GameManager.Instance.gamePaused)
        {
            LockPlayerGameplayInput = true;
        }
        else
        {
            LockPlayerGameplayInput = false;
        }

        if (LockPlayerGameplayInput )
        {
            moveX = 0;
            moveY = 0;
            fire = false;
            stopfire = true;
            jump = false;
            sprintOn = false;
            sprintOff = true;
            //mouseX = 0;
            //mouseY = 0;
            //add more things here - sprint, jump, fire, etc
        }
    }
}
