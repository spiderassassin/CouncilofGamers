using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FirstGearGames.SmoothCameraShaker;
using TMPro;
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
    public bool isGrounded;
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
    public GameObject Highscore;

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
    public AimAssist aimAssist;
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
        simulateGravity();
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



            //simulateGravity();
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


        //aimAssist.gameObject.SetActive(true);
        if(aimAssist.inRange.Count == 0)
        {
            g.Launch(fireballOrigin.forward, velocity);
        }
        else
        {
            //print("detected");
            Collider c = aimAssist.inRange[0];
            var v = FireManager.manager.GetIFlammable(c);
            Vector3 offset = Vector3.zero;
            if (v != null)
            {
                /*offset = v.AimVelocity*.7f;
                offset.y = 0f;*/
                g.Home(c);
            }

            Vector3 targetPos = c.transform.position + offset;
            Vector3 firePos = transform.position;
            Vector3 direction = (targetPos - firePos).normalized;

            /*float x = Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(firePos.x, 0, firePos.z));
            float y = targetPos.y - firePos.y;

            // Calculate the firing angle using the formula for projectile motion
            float angle = CalculateFiringAngle(x, y, 30f, velocity.y * gravity);



           
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y - angle, rotation.eulerAngles.z);

            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector3 force = new Vector3(0, Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians)) * g.GetComponent<Rigidbody>().mass;
            print(force);
            Vector3 rotatedForce = Quaternion.LookRotation(direction) * force;*/
            
            g.Launch(new Vector3(direction.x, fireballOrigin.forward.y, direction.z), velocity);
            //g.Launch(fireballOrigin.forward, velocity);
        }
        //aimAssist.gameObject.SetActive(false);


    }


    float CalculateFiringAngle(float x, float y, float v, float g)
    {
        float v2 = v * v; // v^2
        float v4 = v2 * v2; // v^4
        float gx = g * x; // g * x
        float sqrtTerm = Mathf.Sqrt(v4 - g * (g * x * x + 2 * y * v2));

        if (float.IsNaN(sqrtTerm))
        {
            return float.NaN;
        }

        float angle1 = Mathf.Atan((v2 + sqrtTerm) / gx);
        float angle2 = Mathf.Atan((v2 - sqrtTerm) / gx);

        return Mathf.Min(angle1, angle2) * Mathf.Rad2Deg; // Choose the smaller angle for a more direct shot
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

    bool checkGrounded()
    {

        return Physics.Raycast(transform.position, Vector3.down, 1f, groundMask);

    }

    void simulateGravity()
    {
        //check if player is grounded
        isGrounded = checkGrounded();
        //isGrounded = characterController.isGrounded; // Physics.CheckSphere(groundCheck.position, .05f, groundMask);
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
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.dash, transform.position);
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
        // Reset dash variable.
        dash = false;
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
            armAnimator.SetBool("isDed", true); // make arm animation stop
            
            GetComponentInChildren<CameraBehavior>().Die();
            if (isFiring)
            {
                isFiring = false;
                Fire(false);
            }
            yield return new WaitForSeconds(2);
        }

        Debug.Log("Game Over");
        
        if (GameManager.Instance.endlessMode) {
            // Check for new highscore and update if necessary.
            var scoreText = "Score: " + GameManager.Instance.score;

            if (GameManager.Instance.score > PlayerPrefs.GetInt("Highscore", -1)) {
                PlayerPrefs.SetInt("Highscore", GameManager.Instance.score);
                scoreText = "New Highscore!\n" + scoreText;
            }
           

            Highscore.GetComponentInChildren<TextMeshProUGUI>().text = scoreText;
            Highscore.SetActive(true);

            yield return new WaitForSeconds(3);

            Cursor.lockState = CursorLockMode.None;
            Destroy(CombatUI.Instance);

            // Stop all sounds.
            FMODEvents.Instance.StopAllSounds();
            // Destroy all singletons.
            Destroy(SoundManager.Instance.gameObject);
            Destroy(WaveManager.Instance.gameObject);
            Destroy(CombatUI.Instance.gameObject);
            // Reset fuel amount;
            GameManager.Instance.fuel = 100;
            GameManager.Instance.gameOver = false;
            // Return to menu.
            SceneManager.LoadScene(0);
        } else if (GameManager.Instance.gameStage != GameManager.GameStage.Ending) {
            // Only do something if the game is not in the ending stage.
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
