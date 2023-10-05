using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float abilitySpeed;
    [Tooltip("Used for Player Test Ability Speed Increase per level")]
    [SerializeField] float abilityConstant = 0.3f;
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
    bool isFacingRight = true;

    int abilityLevel = 0;

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

    // Flare Ability variables
    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private float flareForce;
    [SerializeField] private float flareAbilityIncrement = 0.3f;
    private bool didShootFlare;

    // Gargoyle Ability variables
    private MainCameraManager mainCameraManager;
    private bool isPlayerDetectable;
    #endregion


    // Pause Screen
    [SerializeField] private GameObject pauseScreen;
    private bool isPaused;

    // Level Music (independent from sound manager) //
    [SerializeField] private GameObject levelMusic;
    [SerializeField] private GameObject loseMusic;
    [SerializeField] private GameObject darkerMusic;

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
        fuelAmt = MAX_FUEL;
        attackPosition.transform.localPosition = new Vector3(attackDistance, 0, 0);
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        playerAbility = GetComponent<PlayerAbilityHandler>();
        inputHandler = GetComponent<PlayerInputHandler>();
        uISkillHandler = FindAnyObjectByType<UISkillHandler>();
        abilityTimer = abilityDuration;

        // for flare
        didShootFlare = false;

        // for gargoyle
        mainCameraManager = FindAnyObjectByType<MainCameraManager>();
        isPlayerDetectable = true;
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

        if (/*Input.GetKey(KeyCode.J)*/ inputHandler.IsConsume()) // Consume
        {
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

            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(moveInput.x) > 0 ||
                Mathf.Abs(moveInput.y) > 0) // if there is a movement input
            {
                animator.SetBool("IsMoving", true);
                if(!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
            }
            else // not moving
            {
                animator.SetBool("IsMoving", false);
                GetComponent<AudioSource>().Stop();
            }

            if (/*Input.GetKeyDown(KeyCode.U)*/ inputHandler.IsAttack()) // Attack
            {
                animator.SetTrigger("Attack");
            }

            if (/*Input.GetKey(KeyCode.LeftShift)*/ inputHandler.IsSneak()) // Checks if it's sneaking
            {
                isSneaking = true;
                animator.SetBool("IsCrouching", true);
            }
            else if (/*!Input.GetKey(KeyCode.LeftShift)*/ inputHandler.IsSneak() == false)
            {
                isSneaking = false;
                animator.SetBool("IsCrouching", false);
            }

            if (!isSneaking) // if the player is not sneaking then accept ability input
            {
                if (!startAbilityTimer) // if the ability is not activated 
                {
                    if (/*Input.GetKeyDown(KeyCode.I)*/ inputHandler.IsAbility())
                    {
                        abilityLevel = playerAbility.GetAbilityLevel();
                        UseAbility();
                    }
                }
            }

            if (/*Input.GetKeyDown(KeyCode.Space)*/inputHandler.IsDash() && canDash && CheckFuel(dashFuelCost))
            {
                StartCoroutine(Dash());
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
        if(inputHandler.IsCycle())
        {
            playerAbility.CycleSkills();
        }
    }

    void SneakCheck()
    {
        if (isSneaking)
        {
            actualSpeed = sneakSpeed;
            playerLight.intensity = minLight;
            playerLight.spotAngle = minLight;
        }
        else if (playerAbility.GetCurrentAbility() == Ability.Type.TEST && startAbilityTimer)
        {
            Debug.Log("Ability " + playerAbility.GetCurrentAbility() + " is Speed Boost.");

            if (abilityLevel > 0)
            {
                actualSpeed = abilitySpeed + (abilitySpeed * (abilityLevel * abilityConstant));
            }
            else
            {
                actualSpeed = abilitySpeed;
            }
            playerLight.intensity = maxLight;
            playerLight.spotAngle = maxLight;
        }
        else if (playerAbility.GetCurrentAbility() == Ability.Type.SENTRY && startAbilityTimer)
        {
            // should be slower than speed boost abiltiy, right not 70% of speed boost speed
            if (abilityLevel > 0)
            {
                actualSpeed = (abilitySpeed + (abilitySpeed * (abilityLevel * abilityConstant))) * 0.7f; 
                actualSpeed = 20.0f;
            }
            else
            {
                actualSpeed = abilitySpeed * 0.7f;
            }
            playerLight.intensity = maxLight;
            playerLight.spotAngle = maxLight;
        }
        else if (!isSneaking)
        {
            actualSpeed = speed;
            playerLight.intensity = maxLight;
            playerLight.spotAngle = maxLight;
        }
        
    }

    private void Move() 
    {
        rb.velocity = new Vector3
            (
               moveInput.normalized.x * actualSpeed * Time.deltaTime,
               rb.velocity.y,
               moveInput.normalized.y * actualSpeed * Time.deltaTime
            );

        if (!isFacingRight && moveInput.x > 0) // flip to the right 
        {
            Flip();
            attackPosition.transform.localPosition = new Vector3(attackDistance, 0, 0);
        }
        else if(isFacingRight && moveInput.x < 0) // flip to the left 
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
        if(fuelTicks > fuelDecrementInterval)
        {
            if (isSneaking)
            {
                fuelAmt -= fuelSneakDecrementAmt;

            }
            else
            {
                fuelAmt -= fuelDecrementAmt;
            } 
            fuelTicks = 0.0f;
        }
        //Debug.Log("Fuel: " + fuelAmt);
        playerLight.spotAngle = fuelAmt;

        if(fuelAmt < 25.0f && isAlive)
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
        return isFacingRight;
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

                // Check for gargoyle ability
                if(playerAbility.GetCurrentAbility() == Ability.Type.SENTRY)
                {
                    mainCameraManager.ToggleGargoyleFX(false);
                    isPlayerDetectable = true;
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

                abilityTimer -= Time.deltaTime;
                uISkillHandler.SetSkillMeter(abilityTimer/abilityDuration);
            }
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
                loseMusic.GetComponent<AudioSource>().Play();

            animator.SetTrigger("Dead");
            Time.timeScale = 0;
            loseScreen.SetActive(true);
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
            shotFlareLight.range += flareAbilityIncrement * abilityLevel;
            Rigidbody shotFlareRB = shotFlare.GetComponent<Rigidbody>();
            // Just affecting x and y scale
            shotFlare.transform.localScale = new Vector3(shotFlare.transform.localScale.x + (flareAbilityIncrement * abilityLevel), shotFlare.transform.localScale.y + (flareAbilityIncrement * abilityLevel), 1);
            if (isFacingRight)
            {
                shotFlareRB.AddForce(playerCenter.transform.right * flareForce);
            }
            else
            {
                shotFlareRB.AddForce(playerCenter.transform.right * flareForce * -1.0f);
            }
            didShootFlare = true;
        }
    }

    public void GargoyleAbility()
    {
        mainCameraManager.ToggleGargoyleFX(true);
        isPlayerDetectable = false;
    }

    public bool GetIsPlayerDetectable()
    {
        return isPlayerDetectable;
    }
}
 
