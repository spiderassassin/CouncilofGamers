using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehavior : MonoBehaviour
{
    public Transform playerBody; //need to access the parent transform for rotation
    public Animator animator;
    float xrotation = 0f;
    public GameObject hands;
    
    Vector3 handposition;

    //for vertical rotation of the camera

    


    //This method is executed whenever the player moves the mouse to look around
    public void Start()
    {
        handposition = hands.transform.position;
    }

    public void Update()
    {
        
    }

    public void Dash()
    {
        animator.SetBool("isSprinting" , GetComponentInParent<Controller>().dash);
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
    }

    public void Snap()
    {
        animator.SetTrigger("snap");
    }






    public void Look(float mouseX, float mouseY)
    {
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);//vertical rotation

        xrotation -= (mouseY * Time.deltaTime);
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);//restrict rotation

        transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);//horizontal rotation of camera
        Vector3 temp = new Vector3(0, xrotation, 0);
        
        if (xrotation < 69f)
        {
            hands.transform.position = handposition + temp;
        }
        


    }
}
