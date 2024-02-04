using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float mouseX = 0;
    float mouseY = 0;
    float tempmouseX = 0;
    float tempmouseY = 0;

    float horizontal = 0;
    float vertical = 0;
    public float mouseSensitivity = 100f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        


    }

    // Update is called once per frame
    void Update()
    {
        tempmouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 60;//60 is just a multiplier to scale
        tempmouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 60;
        if (tempmouseX != mouseX || tempmouseY != mouseY)
        {
            mouseX = tempmouseX;
            mouseY = tempmouseY;
            GetComponentInChildren<CameraBehavior>().Look(mouseX, mouseY);
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        

        if (horizontal != 0 || vertical != 0)
        {
            GetComponent<Controller>().Move(horizontal, vertical);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                GetComponent<Controller>().sprint = true;
                GetComponentInChildren<CameraBehavior>().Sprint();

            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Controller>().Jump();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            GetComponent<Controller>().sprint = false;
            GetComponentInChildren<CameraBehavior>().Sprint();
        }

    }
}
