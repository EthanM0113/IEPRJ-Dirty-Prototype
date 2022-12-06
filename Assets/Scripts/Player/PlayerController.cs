using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
[System.Serializable]
public class PlayerController : MonoBehaviour
{
    [Header("Speed Variables")]
    [Tooltip("Used for Player Movement Speed")]
    [SerializeField] float speed;
    [Tooltip("Used for Player Sneaking Speed")]
    [SerializeField] float sneakSpeed;
    float actualSpeed = 0;

    [Space(10)]

    [Header("Health Variables")]
    [Tooltip("Health cap of the player")]
    public int maxHealth = 100;
    [Tooltip("Players current health")]
    public int health; // current
    [Space(10)]


    [Header("Light Variables")]
    [Tooltip("Used for Player Maximum Light Strength")]
    [SerializeField] float maxLight = 2f;
    [Tooltip("Used for Player Minimum Light Strength")]
    [SerializeField] float minLight = 1.5f;
    [Tooltip("Reference to the light source")]
    [SerializeField] Light playerLight;
    [Space(10)]


    [Tooltip("Reference to the player sprite")]
    [SerializeField] SpriteRenderer playerSprite; // used to flip the sprite
    [Space(10)]

    [Header("Attack Related")]
    [Tooltip("Position of the attack point")]
    [SerializeField] GameObject attackPosition; // used to move the attack position
    [Tooltip("Distance of attack point from player")]
    [SerializeField] float attackDistance = 0.58f;
    [Space(10)]

    Rigidbody rb;

    bool isSneaking; // used for sneaking
    bool isFacingRight = true;

    Vector2 moveInput; // vector2 containing the players x and y movement

    [Header("Light Resource Variables")]
    // Variables for Light Resource
    public float MAX_FUEL = 2.0f; // match range of light for now
    public float fuelAmt;
    private float fuelTicks;
    [Tooltip("Interval between ticks")]
    [SerializeField] private float fuelDecrementInterval = 10.0f;
    [Tooltip("By how much the fuel with decrement per interval")]
    [SerializeField] private float fuelDecrementAmt = 0.05f;
    [Tooltip("By how much the fuel with decrement per interval while sneaking")]
    [SerializeField] private float fuelSneakDecrementAmt = 0.02f;

    // Variables fo Dashing
    private bool canDash = true;
    private bool isDashing = false;
    private float dashSpeed = 6.7f;
    private float dashDuration = 0.2f;
    private float dashCooldown = 0.6f;
    private Vector3 dashDirection;
    private TrailRenderer tr;
    private float dashFuelCost = 0.13f;

    // Player Combat
    PlayerCombat playerCombat;
    PlayerAbilityHandler playerAbility;
    [SerializeField] ParticleSystem abilityEffects;

    // Variables for Animation
    Animator animator;

    // Checks if the player is alive or dead
    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        isSneaking = false;
        actualSpeed = speed;
        fuelTicks = 0.0f;
        fuelAmt = MAX_FUEL;
        attackPosition.transform.localPosition = new Vector3(attackDistance, 0, 0);
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        playerAbility = GetComponent<PlayerAbilityHandler>();
    }

    private void Update()
    {
        if (isAlive)
        {
            if (isDashing)
            {
                return;
            }

            InputHandler();
            SneakCheck();
            FuelManager();
        }
        else
        {
            animator.SetTrigger("Dead");
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

        if (Input.GetKey(KeyCode.J)) // Consume
        {
            playerAbility.Consume();
            animator.SetBool("IsConsuming", true);

            // Stops movement
            moveInput.x = 0;
            moveInput.y = 0;
        }


        else if (!Input.GetKey(KeyCode.J)) // Not Consuming, can move
        {
            animator.SetBool("IsConsuming", false);
            playerAbility.Unconsume();

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
            }

            if (Input.GetKeyDown(KeyCode.U)) // Attack
            {
                animator.SetTrigger("Attack");
            }

            if (Input.GetKey(KeyCode.LeftShift)) // Checks if it's sneaking
            {
                isSneaking = true;
                animator.SetBool("IsCrouching", true);
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                isSneaking = false;
                animator.SetBool("IsCrouching", false);
            }

            if (!isSneaking) // if the player is not sneaking then accept ability input
            {
                if (Input.GetKey(KeyCode.I))
                {
                    UseAbility(playerAbility.GetCurrentAbility());
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash && CheckFuel(dashFuelCost))
            {
                StartCoroutine(Dash());
            }
        } 
    }

    void SneakCheck()
    {
        if (isSneaking)
        {
            actualSpeed = sneakSpeed;
            playerLight.intensity = minLight;
        }
        else
        {
            actualSpeed = speed;
            playerLight.intensity = maxLight;
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
        playerLight.range = fuelAmt;
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


    void UseAbility(Ability.Type ability)
    {
        if (ability == Ability.Type.TEST)
        {
            abilityEffects.Play();
            actualSpeed = speed * 4;
        }
    }
}
 
