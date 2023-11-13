using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
[System.Serializable]
public class PlayerController : MonoBehaviour
{
    // General Variables
    #region General Variables
    [SerializeField] private GameObject playerCenter;

    [Header("Speed Variables")]
    [Tooltip("Used for Player Movement Speed")]
    [SerializeField] float speed;
    [Tooltip("Used for Player Sneaking Speed")]
    [SerializeField] float sneakSpeed;
    [Tooltip("Used for Player Test Ability Speed")]
    [SerializeField] float beholderSpeed = 200;
    [Tooltip("Used for Player Test Ability Speed Increase per level")]
    [SerializeField] float beholderIncrement = 10f;
    [SerializeField] private float actualSpeed = 0;



    [Space(10)]
    #endregion

    #region Health Variables
    [Header("Health Variables")]
    /*
    [Tooltip("Health cap of the player")]
    public int maxHealth = 100;
    [Tooltip("Players current health")]
    public int health; // current
    */
    [SerializeField] private PlayerHearts playerHearts;
    public int health;
    public int maxHealth;
    [Space(10)]
    #endregion

    #region Light Variables
    [Header("Light Variables")]
    [Tooltip("Used for Player Maximum Light Strength")]
    [SerializeField] float maxLight = 2f;
    [Tooltip("Used for Player Minimum Light Strength")]
    [SerializeField] float minLight = 1.5f;
    [Tooltip("Reference to the light source")]
    [SerializeField] Light playerLight;
    [Space(10)]
    #endregion

    PlayerInputHandler inputHandler;

    [Tooltip("Reference to the player sprite")]
    [SerializeField] SpriteRenderer playerSprite; // used to flip the sprite
    [Space(10)]

    #region Attack Variables
    [Header("Attack Related")]
    [Tooltip("Position of the attack point")]
    [SerializeField] GameObject attackPosition; // used to move the attack position
    [Tooltip("Distance of attack point from player")]
    [SerializeField] float attackDistance = 0.58f;
    [Space(10)]
    #endregion 

    Rigidbody rb;

    bool isSneaking; // used for sneaking
    bool isFacingRight = false;

    [SerializeField] private int abilityLevel = 0;

    int activatedTorchCount = 0;
    [SerializeField] int torchInterval = 3; // every set number of torches activated hint the final room
    Vector2 moveInput; // vector2 containing the players x and y movement


    // Lose Screen
    [SerializeField] private GameObject loseScreen;

    #region Light Resource Variables
    [Header("Light Resource Variables")]
    // Variables for Light Resource
    [SerializeField] private float MAX_FUEL = 2.0f; // match range of light for now
    public float fuelAmt;
    private float fuelTicks;
    [Tooltip("Interval between ticks")]
    [SerializeField] private float fuelDecrementInterval = 10.0f;
    [Tooltip("By how much the fuel with decrement per interval")]
    [SerializeField] private float fuelDecrementAmt = 0.05f;
    [Tooltip("By how much the fuel with decrement per interval while sneaking")]
    [SerializeField] private float fuelSneakDecrementAmt = 0.02f;
    #endregion

    #region Dashing Variables
    // Variables fo Dashing
    private bool canDash = true;
    private bool isDashing = false;
    private float dashSpeed = 6.7f;
    private float dashDuration = 0.2f;
    private float dashCooldown = 0.6f;
    private Vector3 dashDirection;
    private TrailRenderer tr;
    private float dashFuelCost = 0.13f;
    #endregion


    // Player Combat
    PlayerCombat playerCombat;
    PlayerAbilityHandler playerAbility;
    [SerializeField] ParticleSystem abilityEffects;

    // Variables for Animation
    Animator animator;

    // Checks if the player is alive or dead
    public bool isAlive = true;

    #region Ability Variables
    // Ability variables
    [SerializeField] private float abilityDuration = 3f;
    float abilityTimer;
    bool startAbilityTimer = false;
    private UISkillHandler uISkillHandler;
    private FuelBarHandler fuelBarHandler;

    // Flare Ability variables
    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private float flareForce;
    [SerializeField] private float flareAbilityIncrement = 0.3f;
    private bool didShootFlare;

    // Gargoyle Ability variables
    private MainCameraManager mainCameraManager;
    private bool isPlayerDetectable;
    private bool isPlayerVulnerable;
    private float gargoyleSpeed = 140;
    private float gargoyleSpeedIncrement = 20;


    // Tree Ability variables
    [SerializeField] private GameObject shivPrefab;
    [SerializeField] private float shivForce;
    [SerializeField] private float shivAbilityIncrement = 0.6f;
    private float shivIncrement = 20f;
    private bool didShootShiv;
    #endregion

    // Death Variables
    [SerializeField] private ParticleSystem deathParticles;
    
    
    // Input Variables
    //private bool canInput = true;
    private bool preventMovementInput = false;
    private bool preventAttackInput = false;
    private bool canMove = true;

    // Pause Screen
    [SerializeField] private GameObject pauseScreen;
    private bool isPaused;

    // Level Music (independent from sound manager) //
    [SerializeField] private GameObject levelMusic;
    [SerializeField] private GameObject loseMusic;
    [SerializeField] private GameObject darkerMusic;

    // Shadow Hand
    [SerializeField] private GameObject shadowHandPrefab;
    [SerializeField] private AudioSource shadowHandSFXSource;
    [SerializeField] private AudioClip SFX_ShadowHand;
    private bool didGraspPlayer = false;
    private GameObject shadowHand;

    // Shop
    private bool isInShop = false;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        // Set HP
        maxHealth = playerHearts.GetMaxHp();
        health = playerHearts.GetCurrentHp(); 
        rb = GetComponent<Rigidbody>();
        isSneaking = false;
        actualSpeed = speed;
        fuelTicks = 0.0f;
        MAX_FUEL = PlayerDataHolder.Instance.GetMaxFuel();
        fuelAmt = MAX_FUEL;
        attackPosition.transform.localPosition = new Vector3(attackDistance, 0, 0);
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        playerAbility = GetComponent<PlayerAbilityHandler>();
        inputHandler = GetComponent<PlayerInputHandler>();
        uISkillHandler = FindAnyObjectByType<UISkillHandler>();
        fuelBarHandler = FindObjectOfType<FuelBarHandler>();
        abilityTimer = abilityDuration;

        // for flare
        didShootFlare = false;

        // for gargoyle
        mainCameraManager = FindAnyObjectByType<MainCameraManager>();
        isPlayerDetectable = true;
        isPlayerVulnerable = true;

        // for tree
        didShootShiv = false;   

    }

    private void Update()
    {
        health = playerHearts.GetCurrentHp();

        isAlive = CheckAlive();
        if (isAlive)
        {
            if (isDashing)
            {
                return;
            }

            InputHandler();
            SneakCheck();
            FuelManager();
            CheckAbility();           
        }
        else
        {
            animator.SetTrigger("Dead");
            //deathParticles.Play();
            loseScreen.SetActive(true);
            canMove = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Move();
    }
    
    void InputHandler() {

        if (!canMove)
        {
            GetComponent<AudioSource>().Stop();
            moveInput.x = 0;
            moveInput.y = 0;
        }

       
        if (/*Input.GetKey(KeyCode.J)*/ inputHandler.IsConsume()) // Consume
        {
            GetComponent<AudioSource>().Stop(); // Stops walking animation from overlapping
            playerAbility.Consume();
            animator.SetBool("IsConsuming", true);

            // Stops movement
            moveInput.x = 0;
            moveInput.y = 0;
        }
        else if (/*!Input.GetKey(KeyCode.J)*/ inputHandler.IsConsume() == false) // Not Consuming, can move
        {
            animator.SetBool("IsConsuming", false);
            playerAbility.Unconsume();

            if (!preventMovementInput)
            {
                moveInput.x = Input.GetAxisRaw("Horizontal");
                moveInput.y = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(moveInput.x) > 0 ||
                    Mathf.Abs(moveInput.y) > 0) // if there is a movement input
                {
                    animator.SetBool("IsMoving", true);
                }
                else // not moving
                {
                    animator.SetBool("IsMoving", false);
                    GetComponent<AudioSource>().Stop();
                }
            }
            
            if(!preventAttackInput)
            {
                if (/*Input.GetKeyDown(KeyCode.U)*/ inputHandler.IsAttack()) // Attack
                {
                    if (playerCombat.CheckRadius())
                    {
                        GetComponent<AudioSource>().Stop(); // Stops walking animation from overlapping
                        animator.SetTrigger("Attack");

                    }
                    else
                    {
                        GetComponent<AudioSource>().Stop(); // Stops walking animation from overlapping
                        animator.SetTrigger("AttackMiss");
                    }

                }
            }
        
            //if (/*Input.GetKey(KeyCode.LeftShift)*/ inputHandler.IsSneak()) // Checks if it's sneaking
            //{
            //    isSneaking = true;
            //    animator.SetBool("IsCrouching", true);
            //}
            //else if (/*!Input.GetKey(KeyCode.LeftShift)*/ inputHandler.IsSneak() == false)
            //{
            //    isSneaking = false;
            //    animator.SetBool("IsCrouching", false);
            //}

            //if (!isSneaking) // if the player is not sneaking then accept ability input
            //{
            //    if (!startAbilityTimer) // if the ability is not activated 
            //    {
            //        if (/*Input.GetKeyDown(KeyCode.I)*/ inputHandler.IsAbility())
            //        {
            //            abilityLevel = playerAbility.GetAbilityLevel();
            //            UseAbility();
            //        }
            //    }
            //}
            if (!startAbilityTimer) // if the ability is not activated 
            {
                if (/*Input.GetKeyDown(KeyCode.I)*/ inputHandler.IsAbility())
                {
                    //abilityLevel = playerAbility.GetAbilityLevel();
                    UseAbility();
                }

                if (inputHandler.IsCycle())
                {
                    playerAbility.CycleSkills();
                }
            }
            else
            {
                if (inputHandler.IsAbility())
                {
                    uISkillHandler.PlayNotReady();
                }
            }

            if (/*Input.GetKeyDown(KeyCode.Space)*/inputHandler.IsDash() && canDash && CheckFuel(dashFuelCost))
            {
                if (!preventMovementInput)
                {
                    if (moveInput != Vector2.zero)
                    {
                        StartCoroutine(Dash());
                        GetComponent<AudioSource>().Stop(); // Stops walking animation from overlapping
                        animator.SetTrigger("Dash");
                    }
                    
                }       
            }
            else if (inputHandler.IsDash() && canDash && !CheckFuel(dashFuelCost))
            {
                fuelBarHandler.PlayNoFuel();
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!isPaused)
                {
                    PauseGame();
                    
                }
                else
                {
                    UnpauseGame();
                }
            }
        }
    }

    public async void SlowDebuff(float duration, float slowSpeed)
    {
        float end = Time.time + duration;
        while (end > Time.time)
        {
            actualSpeed = slowSpeed;
            await Task.Yield();
        }
        actualSpeed = speed;
    }

    void SneakCheck()
    {

        // Faster intial speed than sentry but scales worse, HOWEVER can reach level 15
        if (playerAbility.GetCurrentAbility() == Ability.Type.TEST && startAbilityTimer)
        {
           
            if (abilityLevel > 0)
            {
                if(abilityLevel > 15)
                    abilityLevel = 15;

                actualSpeed = beholderSpeed + (beholderIncrement * abilityLevel);
            }
            else
            {
                actualSpeed = beholderSpeed;
            }
        }
        // Slower initial speed than beholder ability, but scales better, HOWEVER only reaches level 10
        else if (playerAbility.GetCurrentAbility() == Ability.Type.SENTRY && startAbilityTimer)
        {
           
            if (abilityLevel > 0)
            {
                if (abilityLevel > 10)
                    abilityLevel = 10;

                actualSpeed = gargoyleSpeed + (gargoyleSpeedIncrement * abilityLevel);
            }
            else
            {
                actualSpeed = gargoyleSpeed;
            }
        }
        else if (!isSneaking)
        {
            actualSpeed = speed;
        }

        /* Speed Breakdown at each Level
        Beholder	Sentry
        200			140
        210			160
        220			180	
        230			200						
        240			220
        250			240
        260			260	
        270			280
        280			300
        290
        300
        310
        320
        330
        340
        350
         */
    }

    private void Move() 
    {
        if (!canMove) return;

        if(!isInShop)
        {
            // Play footsteps only during motion
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().volume = 1.0f * SoundManager.Instance.GetSFXMultiplier();
                GetComponent<AudioSource>().Play();
            }
        }
        

        rb.velocity = new Vector3
            (
               moveInput.normalized.x * actualSpeed * Time.deltaTime,
               rb.velocity.y,
               moveInput.normalized.y * actualSpeed * Time.deltaTime
            );

        if (!isFacingRight && moveInput.x < 0) // flip to the right 
        {
            Flip();
            attackPosition.transform.localPosition = new Vector3(attackDistance, 0, 0);
        }
        else if(isFacingRight && moveInput.x > 0) // flip to the left 
        {
            Flip();
            attackPosition.transform.localPosition = new Vector3(-attackDistance, 0, 0);
        }
    }

    private void Flip()
    {
        playerSprite.flipX = isFacingRight;
        isFacingRight = !isFacingRight;
    }

    void FuelManager()
    {
        fuelTicks += Time.deltaTime;
        //if(fuelTicks > fuelDecrementInterval)
        //{
        //    if (isSneaking)
        //    {
        //        fuelAmt -= fuelSneakDecrementAmt;

        //    }
        //    else
        //    {
        //        fuelAmt -= fuelDecrementAmt;
        //    } 
        //    fuelTicks = 0.0f;
        //}

        if (fuelTicks > fuelDecrementInterval)
        {
            fuelAmt -= fuelDecrementAmt;
            fuelTicks = 0.0f;

        }

        playerLight.spotAngle = fuelAmt;

        if (fuelAmt < 25.0f && isAlive)
        {
            if (levelMusic.GetComponent<AudioSource>().isPlaying)
                levelMusic.GetComponent<AudioSource>().Stop();

            if (!darkerMusic.GetComponent<AudioSource>().isPlaying)
                darkerMusic.GetComponent<AudioSource>().Play();
        }

        if (fuelAmt > 25.0f)
        {
            if (darkerMusic.GetComponent<AudioSource>().isPlaying)
                darkerMusic.GetComponent<AudioSource>().Stop();

            if (!levelMusic.GetComponent<AudioSource>().isPlaying)
                levelMusic.GetComponent<AudioSource>().Play();
        }

        // Damage player on low fuel
        if(fuelAmt <= 0f)
        {
            // Track player
            if(shadowHand != null)
                shadowHand.transform.position = shadowHandSFXSource.transform.position;

            if (!didGraspPlayer)
            {
                didGraspPlayer = true;  
                shadowHand = Instantiate(shadowHandPrefab);
                

                // Play Shadow Hands SFX
                shadowHandSFXSource.PlayOneShot(SFX_ShadowHand);
                shadowHand.transform.position = shadowHandSFXSource.transform.position;
                //shadowHand.transform.parent = shadowHandSFXSource.transform;
            }
        }
        if(fuelAmt > 0f)
        {
            if(didGraspPlayer)
                didGraspPlayer = false;
        }
    }

    public void UseFuel(float amount)
    {
        fuelAmt -= amount;
    }

    // checks if player has enough fuel
    public bool CheckFuel(float fuel)
    {
        if (fuelAmt >= fuel)
            return true;
        else
            return false;
    }

    public float GetFuel()
    {
        return fuelAmt;
    }

    
    public bool GetFaceDirection()
    {
        return !isFacingRight;
    }

    private IEnumerator Dash()
    {
        // Set dash variables
        canDash = false;
        isDashing = true;

        // Actual Dash
        UseFuel(dashFuelCost);
        dashDirection = new Vector3(moveInput.x, 0, moveInput.y);
        if (dashDirection == Vector3.zero)
        {
            if (isFacingRight)
                dashDirection = new Vector3(1, 0, 0);
            else
                dashDirection = new Vector3(-1, 0, 0);
        }
        rb.velocity = dashDirection.normalized * dashSpeed;
        tr.emitting = true;

        // During Dash
        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;
        isDashing = false;

        // Dash Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    void UseAbility()
    {
        startAbilityTimer = true;
    }

    void CheckAbility()
    {
        if (startAbilityTimer)
        {
            if (abilityTimer <= 0f)
            {
                abilityTimer = abilityDuration;
                startAbilityTimer = false;
                didShootFlare = false;
                didShootShiv = false;

                // Check for gargoyle ability
                if(playerAbility.GetCurrentAbility() == Ability.Type.SENTRY)
                {
                    preventAttackInput = false;
                    mainCameraManager.ToggleGargoyleFX(false);
                    isPlayerDetectable = true;
                    isPlayerVulnerable = true;
                }

            }
            else
            {
                if (playerAbility.GetCurrentAbility() == Ability.Type.NONE)
                {
                    abilityTimer = abilityDuration;
                    startAbilityTimer = false;
                }
                else if (playerAbility.GetCurrentAbility() == Ability.Type.TEST)
                {
                    abilityEffects.Play();
                }
                else if (playerAbility.GetCurrentAbility() == Ability.Type.FLARE)
                {
                    FlareAbility();
                }
                else if (playerAbility.GetCurrentAbility() == Ability.Type.SENTRY) 
                {
                    GargoyleAbility();
                }
                else if (playerAbility.GetCurrentAbility() == Ability.Type.TREE)
                {
                    TreeAbility();
                }

                abilityTimer -= Time.deltaTime;
                uISkillHandler.SetSkillMeter(abilityTimer/abilityDuration);
            }
        }
    }

    private void TreeAbility()
    {
        // Shoot wooden shiv in 4/8 directions
        if (!didShootShiv)
        {
            SoundManager.Instance.StaggeredShiv(); // SFX inside method is not final!

            if (abilityLevel > 10)
                abilityLevel = 10;

            for (int i = 0; i < 8; i++)
            {
                GameObject stagShiv = Instantiate(shivPrefab, playerCenter.transform);
                Rigidbody stagShivRB = stagShiv.GetComponent<Rigidbody>();
                // Just affecting x and y scale
                stagShiv.transform.localScale = new Vector3(stagShiv.transform.localScale.x + (shivAbilityIncrement * abilityLevel), stagShiv.transform.localScale.y + (shivAbilityIncrement * abilityLevel), 1);

                #region Rotate Each Shiv
                if (i == 0) 
                {
                    stagShivRB.AddForce(playerCenter.transform.right * (shivForce + shivIncrement * abilityLevel) * -1.0f);
                }
                else if (i == 1)
                {
                    stagShivRB.AddForce(playerCenter.transform.right * (shivForce + shivIncrement * abilityLevel));
                }
                else if (i == 2)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = 90;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    stagShivRB.AddForce(playerCenter.transform.forward * (shivForce + shivIncrement * abilityLevel) * -1.0f);
                }
                else if (i == 3)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = 90;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    stagShivRB.AddForce(playerCenter.transform.forward * (shivForce + shivIncrement * abilityLevel));
                }
                else if (i == 4)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = 45;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    Vector3 halfDirection = Quaternion.Euler(0, 45, 0) * transform.forward;
                    stagShivRB.AddForce(halfDirection * (shivForce + shivIncrement * abilityLevel));
                }
                else if (i == 5)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = -45;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    Vector3 halfDirection = Quaternion.Euler(0, 135, 0) * transform.forward;
                    stagShivRB.AddForce(halfDirection * (shivForce + shivIncrement * abilityLevel));
                }
                else if (i == 6)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = 45;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    Vector3 halfDirection = Quaternion.Euler(0, 225, 0) * transform.forward;
                    stagShivRB.AddForce(halfDirection * (shivForce + shivIncrement * abilityLevel));
                }
                else if (i == 7)
                {
                    Vector3 shivRotation = stagShiv.transform.localEulerAngles;
                    shivRotation.z = -45;
                    stagShiv.transform.localEulerAngles = shivRotation;
                    Vector3 halfDirection = Quaternion.Euler(0, 315, 0) * transform.forward;
                    stagShivRB.AddForce(halfDirection * (shivForce + shivIncrement * abilityLevel));
                }
                #endregion
            }

            didShootShiv = true;
        }
    }

    public bool GetSneaking()
    {
        return isSneaking;
    }

    public bool CheckAlive()
    {
        if (health <= 0)
        {
            levelMusic.GetComponent<AudioSource>().Stop();              // stop level music
            darkerMusic.GetComponent<AudioSource>().Stop();

            if (!loseMusic.GetComponent<AudioSource>().isPlaying)       // start game over music, if it isn't already playing
            {
                SoundManager.Instance.GameOver();
                //loseMusic.GetComponent<AudioSource>().volume = SoundManager.Instance.GetMusicMultiplier();
                //loseMusic.GetComponent<AudioSource>().Play();

            }


            //Time.timeScale = 1;
            return false;
        }
        else
        {
            return true;
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        SoundManager.Instance.Pause();
        pauseScreen.SetActive(true);
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        isPaused = false;
    }

    public void SetMaxFuel(float newMaxFuel)
    {
        MAX_FUEL = newMaxFuel;
    }

    public float GetMaxFuel()
    {
        return MAX_FUEL;
    }

    public void FlareAbility()
    {
        if (!didShootFlare)
        {
            SoundManager.Instance.Fireball();
            GameObject shotFlare = Instantiate(flarePrefab, playerCenter.transform);
            Light shotFlareLight = shotFlare.GetComponentInChildren<Light>();

            if(abilityLevel > 10)
                abilityLevel = 10;

            shotFlareLight.range += flareAbilityIncrement * abilityLevel;
            Rigidbody shotFlareRB = shotFlare.GetComponent<Rigidbody>();
            // Affect Scale
            shotFlare.transform.localScale = new Vector3(shotFlare.transform.localScale.x + (flareAbilityIncrement * abilityLevel), shotFlare.transform.localScale.y + (flareAbilityIncrement * abilityLevel), shotFlare.transform.localScale.z + (flareAbilityIncrement * abilityLevel));

            // Update children
            foreach (Transform child in shotFlare.transform)
            {
                Debug.Log("Name Of Child " + child.transform.name);
                child.transform.localScale = new Vector3(child.transform.localScale.x + (flareAbilityIncrement * abilityLevel), child.transform.localScale.y + (flareAbilityIncrement * abilityLevel), child.transform.localScale.z + (flareAbilityIncrement * abilityLevel));
                foreach (Transform child2 in child.transform)
                {
                    Debug.Log("Name Of Child 2 " + child2.transform.name);
                    child2.transform.localScale = new Vector3(child2.transform.localScale.x + (flareAbilityIncrement * abilityLevel), child2.transform.localScale.y + (flareAbilityIncrement * abilityLevel), child2.transform.localScale.z + (flareAbilityIncrement * abilityLevel));
                }
            }
            
            if (isFacingRight)
            {
                shotFlareRB.AddForce(playerCenter.transform.right * (flareForce + 10 * abilityLevel) * -1.0f);
            }
            else
            {
                shotFlareRB.AddForce(playerCenter.transform.right * (flareForce + 10 * abilityLevel));
            }

            didShootFlare = true;

            /*  Speed Breakdown
                220
                240
                260
                280
                300
                320
                340
                360
                380
                400
             */
        }
    }

    public void GargoyleAbility()
    {
        preventAttackInput = true;
        mainCameraManager.ToggleGargoyleFX(true);
        isPlayerDetectable = false;
        isPlayerVulnerable = false;
    }

    public bool GetIsPlayerDetectable()
    {
        return isPlayerDetectable;
    }

    public bool GetIsPlayerVulnerable()
    {
        return isPlayerVulnerable;
    }

    public void PlayDeathParticles()
    {
        deathParticles.Play();
    }

    public void SetPreventMovementInput(bool flag)
    {
        preventMovementInput = flag;
    }

    public void SetPreventAttackInput(bool flag)
    {
        preventAttackInput = flag;
    }

    public void CanMove()
    {
        canMove = !canMove;
    }

    public bool IncrementTorchCount()
    {
        if (fuelAmt < 0) return false;
        activatedTorchCount++;
        if (activatedTorchCount % torchInterval == 0)
        {
            return true;
        }
        return false;
    }

    public int GetAbilityLevel()
    {
        return abilityLevel;    
    }

    public void SetIsInShop(bool flag)
    {
        isInShop = flag;
    }

    public bool GetIsInShop()
    {
        return isInShop;
    }
}
 
