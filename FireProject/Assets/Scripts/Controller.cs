using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FirstGearGames.SmoothCameraShaker;
public class Controller : Entity
{
    public static Controller Instance;

    public CharacterController characterController;
    public float speed = 10f;
    public float dashSpeed;
    public float dashTime;
    public int dashCooldownTime;
    private Task dashCooldown = null;
    public Vector3 velocity;
    public float gravity = -9.81f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;
    public bool dash = false;
    public bool isFiring = false;
    public bool isSnapping = false;
    public AudioSource source;
    public DamageInformation snapDamage;


    public bool snapAllowed;
    public bool punchAllowed;
    public bool flameAttackAllowed;
    public bool fireballAllowed;




    public GameObject GameOver;

    public bool isMoving = false;

    public AudioClip playerwalk;
    public AudioClip playerrun;
    public AudioClip punch;
    public AudioClip fire;
    public AudioClip snap, failedSnap;
    public AudioClip playerDamage;
    public AudioClip fireBall;
    public AudioClip slowmotion;

    private EventInstance flame;
    private EventInstance walk;
    private EventInstance run;



    public FireSource firesource;
    public ParticleSystem coneFireSystem;
    public FireSource punchSource;
    public Fireball fireballPrefab;
    public Transform fireballOrigin;
    public float fireballCooldown = .5f;
    public float punchCooldown = .2f;
    public float healthIncreaseRate = 1;
    bool dead = false;
    public Image rightArm;
    public Color punchCooldownColour;
    
    public Animator armAnimator;

    private float lastFireballTime;
    private float lastPunchTime;

    public bool invincibility;
    float invincibilityDurationTimer = 0;
    public float invincibilityDuration = 0.25f;

    public Vector3 additive;


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

        punchSource.Initialize(this, null);
        firesource.Initialize(this, null);
    }
    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        //flame = SoundManager.Instance.CreateInstance(FMODEvents.Instance.flamethrower);
        walk = SoundManager.Instance.CreateInstance(FMODEvents.Instance.run);
        //run = SoundManager.Instance.CreateInstance(FMODEvents.Instance.run);
        
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(flame, gameObject.transform, GetComponent<Rigidbody>());
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(walk, transform);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(run, transform);


    }

    private float GetDamageMultiplier(float adrenalinePercent)
    {
        return 1 + adrenalinePercent;
    }

    void Update()
    {
        //print(flame.get3DAttributes())
        if (GameManager.Instance.gamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
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
            if (currentHealth < 100)
            {
                currentHealth += Time.deltaTime * healthIncreaseRate * (100 - currentHealth);
                //health += Time.deltaTime * healthIncreaseRate * (1/Mathf.Log(health+1));
               
                if (currentHealth > 100)
                {
                    currentHealth = 100;
                }
            }


            source = this.GetComponent<AudioSource>();


            GetComponentInChildren<CameraBehavior>().Look(InputManager.Instance.mouseX, InputManager.Instance.mouseY);//camera rotation
            dash = InputManager.Instance.dash;
            if (InputManager.Instance.moveX != 0 || InputManager.Instance.moveY != 0)
            {
                if (isMoving == false)
                {
                    isMoving = true;
                    //audio logic for walk sound effect
                    walk = SoundManager.Instance.CreateInstance(FMODEvents.Instance.run);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(walk, transform);
                    walk.start();
                    walk.release();

                }

                Move(InputManager.Instance.moveX, InputManager.Instance.moveY);
            }
            else
            {
                if (isMoving == true)
                {
                    isMoving = false;
                }

                walk.stop(STOP_MODE.ALLOWFADEOUT);
                // Even if the player is not moving, they can still dash.
                if (dash)
                {
                    Dash(transform.forward);
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

            if (InputManager.Instance.fireball && GameManager.Instance.fuel > 0 && fireballAllowed)
            {
                Fireball();
            }

            if (InputManager.Instance.punch)
            {
                StartCoroutine(Punch());
            }

            if (InputManager.Instance.fire && GameManager.Instance.fuel > 0 && flameAttackAllowed)
            {
                GameManager.Instance.UpdateFuel(isFiring, false); // decrease the fuel
                if (isFiring == false)
                {
                    armAnimator.SetBool("isFlamethrowing", true); // animator trigger

                    Fire(true);
                    isFiring = true;
                    
                    
                }

            }
            if (InputManager.Instance.stopfire || GameManager.Instance.fuel <= 0)
            {
                armAnimator.SetBool("isFlamethrowing", false); // animator un-trigger

                Fire(false);
                isFiring = false;


               


            }


            if (InputManager.Instance.snap && snapAllowed)
            {
                 StartCoroutine(Snap());
                //Snap();
            }

            // Reset right arm colour if punch off cooldown.
            if (Time.timeSinceLevelLoad - lastPunchTime >= punchCooldown) {
                rightArm.color = new Color(1, 1, 1, 1);
            }



            simulateGravity();
        }

        if(!InputManager.Instance.LockPlayerGameplayInput)
            characterController.Move(additive * Time.deltaTime);
    }

    IEnumerator Punch()
    {
        if (Time.timeSinceLevelLoad - lastPunchTime < punchCooldown) yield break;
        if (!punchAllowed) yield break;
        lastPunchTime = Time.timeSinceLevelLoad;
        punchSource.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);
        
        //SoundManager.Instance.PlaySoundOnce(punch, transform);
        
        armAnimator.SetTrigger("punch");// animator trigger
        // Recolour right arm if punch is on cooldown.
        yield return new WaitForSeconds(0.2f);
        punchSource.Damage();
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.punch, transform.position);
        rightArm.color = punchCooldownColour;
        //GameManager.Instance.fuel += 10; // NOT FINAL
        //GameManager.Instance.fuel = Mathf.Clamp(GameManager.Instance.fuel, 0, 100); // FOR SURE DEFO NOT FINAL
        

        //punchSource.SetActive(false);


    }

    void Fire(bool active)
    {
        
        firesource.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);

        firesource.SetActive(active);
        if (!active)
        {
            coneFireSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            flame.stop(STOP_MODE.ALLOWFADEOUT);
            
            //NEED TO FIX FADES

        }
        else
        {
            coneFireSystem.Play(true);
            flame = SoundManager.Instance.CreateInstance(FMODEvents.Instance.flamethrower);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(flame, gameObject.transform);
            flame.start();
            flame.release();
        }
    }

    void Fireball()
    {
        if (Time.timeSinceLevelLoad - lastFireballTime < fireballCooldown) return;
        lastFireballTime = Time.timeSinceLevelLoad;
        GameManager.Instance.UpdateFuel(isFiring, true); // decrease the fuel
        Fireball g = Instantiate(fireballPrefab.gameObject).GetComponent<Fireball>();
        g.Initialize(this, null);
        g.gameObject.SetActive(false);
        armAnimator.SetTrigger("isThrow"); // animator trigger
        //SoundManager.Instance.PlaySoundOnce(fireBall, transform);
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.fireball, transform.position);
        g.DamageMultiplier = GetDamageMultiplier(GameManager.Instance.AdrenalinePercent);
        g.transform.position = fireballOrigin.position;
        CameraShakerHandler.Shake(GameManager.Instance.firballShake);
        //CameraShakerHandler.Shake(GameManager.Instance.snapShake);
        g.Launch(fireballOrigin.forward, velocity);
    }

    public bool canSnap()
    {
        return GameManager.Instance.AdrenalinePercent >= 0.98f;
    }

    IEnumerator Snap()
    {
        

        
        

        if (canSnap() && (isSnapping == false))
        {
            isSnapping = true;
            //Time.timeScale = 0.1f;
            //Time.fixedDeltaTime = 0.1f * 0.02f;
            GetComponentInChildren<CameraBehavior>().Snap();
            //SoundManager.Instance.MusicStop();
            //SoundManager.Instance.PlaySoundOnce(slowmotion, transform);
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.slowmotion, transform.position);
            CombatUI.Instance.lerptogrey();
            
            yield return new WaitForSeconds(0.5f);
            
            armAnimator.SetTrigger("snap");
            
        }
        else
        {
            //armAnimator.SetTrigger("snap");// animator trigger; you snap even if it does nothing (for now)
            yield return new WaitForSeconds(0f);
        }
    }
    public IEnumerator SnapLogic()
    {
        
            //Time.timeScale = 1;
            //Time.fixedDeltaTime = 0.02f;
            CombatUI.Instance.snap();
            //SoundManager.Instance.PlaySoundOnce(snap, transform);
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.snap, transform.position);
            // Indicate that a snap occurred in the game manager.
            GameManager.Instance.snapped = true;
            
            
            FireManager.manager.StepFireLevel(this, snapDamage);
            CombatUI.Instance.lerptocolor();
            yield return new WaitForSeconds(1f);
            //SoundManager.Instance.MusicPlay();
            
            isSnapping = false;


        
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
        bool dashed = false;
        if (dash)
        {
            dashed = Dash(move);
        }
        if (!dashed)
        {
            characterController.Move(move * speed * Time.deltaTime);
        }
    }

    // Returns true if the dash was successful, false otherwise.
    bool Dash(Vector3 move)
    {
        // Check for the cooldown.
        if (dashCooldown != null && dashCooldown.Running) {
            return false;
        }

        GetComponentInChildren<CameraBehavior>().Dash();
        StartCoroutine(DashCoroutine(move));
        return true;
    }

    private IEnumerator DashCoroutine(Vector3 move)
    {
        // Restart cooldown.
        dashCooldown = new Task(dashCooldownTime);
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            characterController.Move(move * dashSpeed * Time.deltaTime);
            yield return null; // Stop here and continue next frame.
        }
        GetComponentInChildren<CameraBehavior>().Dash();
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

        GameManager.Instance.gameOver = true;
        
        if (playerDeath)
        {
            
            GetComponentInChildren<CameraBehavior>().Die();
            if (isFiring)
            {
                isFiring = false;
                Fire(false);
            }
            yield return new WaitForSeconds(2);
        }

        Debug.Log("Game Over");
        

        // Don't do anything if the game is in the ending stage.
        if (GameManager.Instance.gameStage != GameManager.GameStage.Ending) {
            GameOver.SetActive(true);

            yield return new WaitForSeconds(3);
            //SoundManager.Instance.MusicStop();
            Cursor.lockState = CursorLockMode.None;
            Destroy(CombatUI.Instance);

            // Stop all sounds.
            FMODEvents.Instance.StopAllSounds();
            // Destroy all singletons.
            Destroy(SoundManager.Instance.gameObject);
            Destroy(WaveManager.Instance.gameObject);
            //Destroy(InputManager.Instance.gameObject);
            Destroy(FMODEvents.Instance.gameObject);
            //Destroy(Controller.Instance.gameObject);
            Destroy(CombatUI.Instance.gameObject);
            // Reset fuel amount;
            GameManager.Instance.fuel = 100;
            // Restart from the beginning of the current stage.
            GameManager.Instance.gameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            // Indicate that the player has died during the credits.
            GameManager.Instance.gameOver = false;
            GameManager.Instance.gameEndDeath = true;
        }
    }

    Coroutine pushback=null;
    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        if ((dead == false && (invincibility == false||dmg.passIFrames)) && (isSnapping == false))
        {
            invincibility = true;
            invincibilityDurationTimer = 0;
            CombatUI.Instance.DamageOverlay();
            //SoundManager.Instance.PlaySoundOnce(playerDamage, transform);
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.playerDamage, transform.position);
            currentHealth -= dmg.damage;

            if (currentHealth <= 0)
            {
                dead = true;

                StartCoroutine(Die());
            }
            else
            {
                if (dmg.pushBack != 0)
                {
                    Vector3 knockbackDirection = (transform.position - attacker.Position).normalized;
                    knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
                    if (pushback != null)
                    {
                        StopCoroutine(pushback);
                    }
                    pushback = StartCoroutine(Pushback(knockbackDirection * dmg.pushBack));
                }
            }
        }
    }
    IEnumerator Pushback(Vector3 impulse)
    {
        additive += impulse*7f;
        while (true)
        {
            additive += -additive.normalized*Time.deltaTime*50f;
            yield return new WaitForEndOfFrame();

            if (additive.magnitude <= .1f)
            {
                additive = Vector3.zero;
                break;
            }
        }
        pushback = null;
    }


}
