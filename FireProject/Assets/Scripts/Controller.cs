using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : Entity
{
    public CharacterController characterController;
    public float speed = 10f;
    public float sprintspeed = 35f;
    public Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;
    public bool sprint = false;
    public AudioSource source;
    public int adrenaline;
  
    
    


    private void Start()
    {
        source = this.GetComponent<AudioSource>();
        Debug.Log(adrenaline);
    }
    // Update is called once per frame
    void Update()
    {
        
        //check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //simulate gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);//it is multiplied by deltatime twice becuase v = 1/2g*tˆ2

        



}
    public void Move(float horizontal, float vertical)
    {

       
            Vector3 move = transform.right * horizontal + transform.forward * vertical;//horizontal and vertical movement of the player
            if (sprint)
            {
                characterController.Move(move * sprintspeed * Time.deltaTime);
                SoundManager.Instance.Play(SoundManager.Instance.playerrun, source);
            }
            else
            {
                characterController.Move(move * speed * Time.deltaTime);
                SoundManager.Instance.Play(SoundManager.Instance.playerwalk, source);
            }
        
       
        
            
        
        
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //SoundManager.Instance.Play(SoundManager.Instance.jump, source);
            adrenaline += 10;
        }
    }



    void Attack()
    {

    }

    public void OnDamage() {
        CombatUI.Instance.DamageOverlay();

    }



    
}
