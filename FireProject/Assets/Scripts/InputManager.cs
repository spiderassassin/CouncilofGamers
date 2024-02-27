using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool dialouge => jumpAction.WasPerformedThisFrame();

    public bool startwave = false;//this is for testing only, to start waves

    private void Awake()
    {
        sprintOff = true;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        if (LockPlayerGameplayInput) return;
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
        if (LockPlayerGameplayInput) return;
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
        if (LockPlayerGameplayInput) return;
        jump = obj.ReadValueAsButton();
    }

    void Start()
    {
         
    }

    
    void Update()
    {
        startwave = Input.GetKeyDown(KeyCode.M)&&!LockPlayerGameplayInput;
    }
}
