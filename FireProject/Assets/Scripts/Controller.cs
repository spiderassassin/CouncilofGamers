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
    public bool isSnapping = false;
    public AudioSource source;
    public DamageInformation snapDamage;



    


    public bool isMoving = false;

    public AudioClip playerwalk;
    public AudioClip playerrun;
    public AudioClip punch;
    public AudioClip fire;
    public AudioClip snap, failedSnap;
    public AudioClip playerDamage;
    public AudioClip fireBall;
    public AudioClip slowmotion;


    GameObject obj;
    GameObject fireAudio;
    public FireSource firesource;
    public ParticleSystem coneFireSystem;
    public FireSource punchSource;
    public Fireball fireballPrefab;
    public Transform fireballOrigin;
    public float fireballCooldown = .5f;
    public float healthIncreaseRate = 1;
    bool dead = false;
    
    public Animator armAnimator;

    private float lastFireballTime;

    public bool invincibility;
    float invincibilityDurationTimer = 0;
    public float invincibilityDuration = 0.25f;

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
        //TEMP FIX CHANGE IT LATER
        SoundManager.Instance.MusicPlay();
    }

    private float GetDamageMultiplier(float adrenalinePercent)
    {
        return 1 + adrenalinePercent;
    }

    void Update()

    {
        if(invincibility == true) {
            invincibilityDurationTimer += Time.deltaTime;
            if (invincibilityDurationTimer > invincibilityDuration)
            {
                invincibilityDurationTimer = 0;
                invincibility = false;

            }
        }


        if (dead == false)
        {
            if (health < 100)
            {
                health += Time.deltaTime * healthIncreaseRate;
            }


            source = this.GetComponent<AudioSource>();


            GetComponentInChildren<CameraBehavior>().Look(InputManager.Instance.mouseX, InputManager.Instance.mouseY);//camera rotation
            if (InputManager.Instance.moveX != 0 || InputManager.Instance.moveY != 0)
            {
                if (isMoving == false)
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
                if (isMoving == true)
                {
                    isMoving = false;
                    SoundManager.Instance.StopSoundEffect(obj);

                }

                if (sprint == true)
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

                if (isFiring == false)
                {
                    armAnimator.SetBool("isFlamethrowing", true); // animator trigger

                    Fire(true);
                    isFiring = true;

                    fireAudio = SoundManager.Instance.PlaySoundloop(fire, transform);
                }

            }
            if (InputManager.Instance.stopfire)
            {
                armAnimator.SetBool("isFlamethrowing", false); // animator un-trigger

                Fire(false);
                isFiring = false;


                SoundManager.Instance.StopSoundEffect(fireAudio);


            }


            if (InputManager.Instance.snap)
            {
                 StartCoroutine(Snap());
                //Snap();
            }



            simulateGravity();
        }


    }

    void Punch()
    {
        punchSource.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);
        punchSource.Damage();
        SoundManager.Instance.PlaySoundOnce(punch, transform);
        armAnimator.SetTrigger("punch");// animator trigger
        

        //punchSource.SetActive(false);


    }

    void Fire(bool active)
    {
        
        firesource.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);

        firesource.SetActive(active);
        if (!active)
            coneFireSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        else
            coneFireSystem.Play(true);
    }

    void Fireball()
    {
        if (Time.timeSinceLevelLoad - lastFireballTime < fireballCooldown) return;

        lastFireballTime = Time.timeSinceLevelLoad;
        Fireball g = Instantiate(fireballPrefab.gameObject).GetComponent<Fireball>();
        g.gameObject.SetActive(false);
        armAnimator.SetTrigger("isThrow"); // animator trigger
        SoundManager.Instance.PlaySoundOnce(fireBall, transform);
        g.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);
        g.transform.position = fireballOrigin.position;
        g.Launch(fireballOrigin.forward);
    }

    bool canSnap()
    {
        return GameManager.Instance.AdrenalinePercent >= 1f;
    }

    IEnumerator Snap()
    {
        

        
        

        if (canSnap())
        {
            isSnapping = true;
            GetComponentInChildren<CameraBehavior>().Snap();
            SoundManager.Instance.MusicStop();
            SoundManager.Instance.PlaySoundOnce(slowmotion, transform);
            CombatUI.Instance.lerptogrey();
            yield return new WaitForSeconds(1f);
            armAnimator.SetTrigger("snap");
            
        }
        else
        {
            armAnimator.SetTrigger("snap");// animator trigger; you snap even if it does nothing (for now)
            yield return new WaitForSeconds(0f);
        }
    }
    public IEnumerator SnapLogic()
    {
        if (canSnap())
        {
            CombatUI.Instance.snap();
            SoundManager.Instance.PlaySoundOnce(snap, transform);
            // Indicate that a snap occurred in the game manager.
            GameManager.Instance.snapped = true;
            
            
            FireManager.manager.StepFireLevel(this, snapDamage);
            CombatUI.Instance.lerptocolor();
            yield return new WaitForSeconds(2f);
            SoundManager.Instance.MusicPlay();
            isSnapping = false;


        }
        else
        {
            SoundManager.Instance.PlaySoundOnce(failedSnap, transform); // For clarity.
            yield return new WaitForSeconds(0f);
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
            if (GameManager.Instance.dialogueState)
        {
            
            return;
        }
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
            armAnimator.SetTrigger("isJump"); // animator trigger
            //SoundManager.Instance.Play(SoundManager.Instance.jump, source);
            //GameObject.Find("GameManager").GetComponent<GameManager>().adrenaline += 10;
        }
    }
    public IEnumerator Die(bool playerDeath = true)
    {
        if (playerDeath)
        {
            GetComponentInChildren<CameraBehavior>().Die();
            yield return new WaitForSeconds(5);
        }

        Debug.Log("Game Over");
        yield return new WaitForSeconds(2);
        SoundManager.Instance.MusicStop();
        Cursor.lockState = CursorLockMode.None;
        Destroy(CombatUI.Instance);

        SceneManager.LoadScene(0);

    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        if ((dead == false && invincibility == false) && (isSnapping = false))
        {
            invincibility = true;
            CombatUI.Instance.DamageOverlay();
            SoundManager.Instance.PlaySoundOnce(playerDamage, transform);
            health -= 10;

            if (health <= 0)
            {
                dead = true;

                StartCoroutine(Die());
            }
        }
    }

    
}
