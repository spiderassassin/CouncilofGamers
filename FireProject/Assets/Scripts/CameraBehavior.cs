using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform playerBody; //need to access the parent transform for rotation
    public Animator animator;
    float xrotation = 0f; //for vertical rotation of the camera

    

    //This method is executed whenever the player moves the mouse to look around
    public void Sprint()
    {
        animator.SetBool("isSprinting" , GetComponentInParent<Controller>().sprint);
    }

    public void Look(float mouseX, float mouseY)
    {
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);//vertical rotation

        xrotation -= (mouseY * Time.deltaTime);
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);//restrict rotation

        transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);//horizontal rotation of camera


    }
}
