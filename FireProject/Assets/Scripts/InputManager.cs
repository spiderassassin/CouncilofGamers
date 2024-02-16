using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance; //Singleton

    public float mouseX = 0;
    public float mouseY = 0;
    

    public float horizontal = 0;
    public float vertical = 0;
    public float mouseSensitivity = 100f;
    public bool spaceDown = false;
    public bool shiftDown = false;
    public bool shiftUp = false;
    public bool takeDamage = false;
    public bool punch = false;

    public bool fire = false;
    public bool stopfire = false;//called when user releases input
    public bool snap = false;
    public bool startwave = false;//this is for testing only, to start waves



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 60;//60 is just a multiplier to scale
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 60;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        spaceDown = Input.GetKeyDown(KeyCode.Space);
        shiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        shiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        takeDamage = Input.GetKeyDown(KeyCode.J);
        punch = Input.GetKeyDown(KeyCode.P);
        fire = Input.GetKeyDown(KeyCode.I);
        stopfire = Input.GetKeyUp(KeyCode.I);
        snap = Input.GetKeyDown(KeyCode.O);
        startwave = Input.GetKeyDown(KeyCode.M);
    }
}
