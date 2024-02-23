using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Controller : Entity
{
    public static Controller Instance;

    public CharacterController characterController;
    public float speed = 10f;
    public float sprintspeed = 35f;
    public Vector3 velocity;
    public float gravity = -9.81f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;
    public bool sprint = false;
    public bool isFiring = false;
    public AudioSource source;

  
    public bool isMoving = false;

    public AudioClip playerwalk;
    public AudioClip playerrun;
    public AudioClip punch;
    public AudioClip fire;
    public AudioClip snap;

    GameObject obj;
    GameObject fireAudio;
    public FireSource firesource;
    public ParticleSystem coneFireSystem;
    public FireSource punchSource;
    public Fireball fireballPrefab;
    public Transform fireballOrigin;
    public float healthIncreaseRate = 1;
    public int snapEffect = 30;

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
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (health < 100)
        {
            health += Time.deltaTime * healthIncreaseRate;
        }


        source = this.GetComponent<AudioSource>();


        GetComponentInChildren<CameraBehavior>().Look(InputManager.Instance.mouseX, InputManager.Instance.mouseY);//camera rotation
        if (InputManager.Instance.moveX != 0 || InputManager.Instance.moveY != 0)
        {
            if(isMoving == false)
            {
                isMoving = true;
                obj = SoundManager.Instance.PlaySoundloop(playerwalk, transform);

            }

            Move(InputManager.Instance.moveX, InputManager.Instance.moveY);

            if (InputManager.Instance.sprintOn)
            {
                if (sprint == false)
                {
                    sprint = true;
                    SoundManager.Instance.StopSoundEffect(obj);
                    obj = SoundManager.Instance.PlaySoundloop(playerrun, transform);

                    GetComponentInChildren<CameraBehavior>().Sprint();
                }

            }
            else if (InputManager.Instance.sprintOff)
            {
                if (sprint == true)
                {

                    sprint = false;
                    SoundManager.Instance.StopSoundEffect(obj);
                    obj = SoundManager.Instance.PlaySoundloop(playerwalk, transform);
                    GetComponentInChildren<CameraBehavior>().Sprint();
                }

            }
        }

        else
        {
            if(isMoving == true)
            {
                isMoving = false;
                SoundManager.Instance.StopSoundEffect(obj);

            }

            if(sprint == true)
            {
                
                sprint = false;
                GetComponentInChildren<CameraBehavior>().Sprint();

            }

           
        }

        if (InputManager.Instance.jump)
        {
            Jump();

        }

        if (InputManager.Instance.takeDamage)
        {
            //OnDamaged();
        }

        if (InputManager.Instance.fireball)
        {
            Fireball();
        }

        if (InputManager.Instance.punch)
        {
            Punch();
        }

        if (InputManager.Instance.fire)
        {
            Fire(true);
            if(isFiring == false)
            {
                isFiring = true;
                fireAudio = SoundManager.Instance.PlaySoundloop(fire, transform);
            }
            
        }
        if (InputManager.Instance.stopfire)
        {
            Fire(false);
            isFiring = false;
            
            SoundManager.Instance.StopSoundEffect(fireAudio);
            
            
        }


        if (InputManager.Instance.snap)
        {
            StartCoroutine(Snap());
            
        }



        simulateGravity();


    }




    void Punch()
    {
        //punchSource.SetActive(true);
        punchSource.Damage();
        SoundManager.Instance.PlaySoundOnce(punch, transform);

        //punchSource.SetActive(false);


    }

    void Fire(bool active)
    {
        firesource.SetActive(active);
        if (!active)
            coneFireSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        else
            coneFireSystem.Play(true);
    }

    void Fireball()
    {
        Fireball g = Instantiate(fireballPrefab.gameObject).GetComponent<Fireball>();
        g.transform.position = fireballOrigin.position;
        g.Launch(fireballOrigin.forward);
    }

    IEnumerator Snap()
    {
        print("SNAP");
        if (GameManager.Instance.adrenaline >= GameManager.Instance.MAX_ADRENALINE)

        {
            SoundManager.Instance.PlaySoundOnce(snap, transform);
            GameManager.Instance.SnapMultiplier = snapEffect;
            yield return new WaitForSeconds(5f);

            GameManager.Instance.SnapMultiplier = 1;


            //logic for snap goes here
        }


    }

    void simulateGravity()
    {
        //check if player is grounded
        isGrounded = characterController.isGrounded; // Physics.CheckSphere(groundCheck.position, .05f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //simulate gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);//it is multiplied by deltatime twice becuase v = 1/2g*tˆ2

    }

    void Move(float horizontal, float vertical)
    {

       
            Vector3 move = transform.right * horizontal + transform.forward * vertical;//horizontal and vertical movement of the player
            if (sprint)
            {
                characterController.Move(move * sprintspeed * Time.deltaTime);
                //SoundManager.Instance.Play(SoundManager.Instance.playerrun, source);
            }
            else
            {
                characterController.Move(move * speed * Time.deltaTime);
                //SoundManager.Instance.Play(SoundManager.Instance.playerwalk, source);
            }
     
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //SoundManager.Instance.Play(SoundManager.Instance.jump, source);
            //GameObject.Find("GameManager").GetComponent<GameManager>().adrenaline += 10;
        }
    }


    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        CombatUI.Instance.DamageOverlay();
        //health -= 10;

        if (health <= 0)
        {
            Debug.Log("Game Over");
            SoundManager.Instance.MusicStop();
            Cursor.lockState = CursorLockMode.None;
            Destroy(CombatUI.Instance);

            SceneManager.LoadScene(0);
        }
    }

    
}
