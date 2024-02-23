using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance; //Singleton

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
    public bool punch => punchAction.WasPerformedThisFrame();
    public bool fireball => fireballAction.WasPerformedThisFrame();

    public bool fire = false;
    public bool stopfire = false;//called when user releases input
    //public bool snap =>snapAction.WasPerformedThisFrame();
    public bool snap = false;

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
        if (!toggleSprint)
        {
            sprintOn = false;
            sprintOff = true;
        }
    }

    private void SprintAction_performed(InputAction.CallbackContext obj)
    {
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
        stopfire = true;
        fire = false;
    }

    private void FireAction_performed(InputAction.CallbackContext obj)
    {
        stopfire = false;
        fire = true;
    }

    private void MoveAction_performed(InputAction.CallbackContext obj)
    {
        moveX = obj.ReadValue<Vector2>().x;
        moveY = obj.ReadValue<Vector2>().y;
    }
    private void MoveAction_canceled(InputAction.CallbackContext obj)
    {
        moveX = 0;
        moveY = 0;
    }
    private void LookAction_canceled(InputAction.CallbackContext obj)
    {
        mouseX = 0;
        mouseY = 0;
    }
    private void JumpAction_canceled(InputAction.CallbackContext obj)
    {
        jump = false;
    }
    private void LookAction_performed(InputAction.CallbackContext obj)
    {
        mouseX = obj.ReadValue<Vector2>().x*mouseSensitivity.x;
        mouseY = obj.ReadValue<Vector2>().y*mouseSensitivity.y;
    }
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        jump = obj.ReadValueAsButton();
    }

    void Start()
    {
         
    }

    
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity.x * 60;//60 is just a multiplier to scale
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity.y * 60;

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        
        
        //sprintOn = Input.GetKeyDown(KeyCode.LeftShift);
        //sprintOff = Input.GetKeyUp(KeyCode.LeftShift);

        //jump = Input.GetKeyDown(KeyCode.Space);
        //takeDamage = Input.GetKeyDown(KeyCode.J);
        // punch = Input.GetKeyDown(KeyCode.P);
        //fire = Input.GetKeyDown(KeyCode.I);
        //stopfire = Input.GetKeyUp(KeyCode.I);
        snap = Input.GetKeyDown(KeyCode.O);
        startwave = Input.GetKeyDown(KeyCode.M);
    }
}
