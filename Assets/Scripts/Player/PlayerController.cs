using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sneakSpeed;
    float actualSpeed = 0;

    public int maxHealth = 100;
    public int health; // current

    [SerializeField] float maxLight = 2f;
    [SerializeField] float minLight = 1.5f;

    [SerializeField] Light playerLight;
    [SerializeField] SpriteRenderer playerSprite; // used to flip the sprite
    [SerializeField] GameObject attackPosition; // used to move the attack position
    [SerializeField] float attackDistance = 0.58f;

    Rigidbody rb;

    bool isSneaking; // used for sneaking
    bool isFacingRight = true;

    Vector2 moveInput; // vector2 containing the players x and y movement

    // Variables for Light Resource
    public float MAX_FUEL = 2.0f; // match range of light for now
    public float fuelAmt;
    private float fuelTicks;
    [SerializeField] private float fuelDecrementInterval = 10.0f;
    [SerializeField] private float fuelDecrementAmt = 0.05f;
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
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        InputHandler();
        SneakCheck();
        FuelManager();
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
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) {
            isSneaking = true;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            isSneaking = false;
        }

        if (!isSneaking) // if the player is not sneaking then accept ability input
        {
            // ability input
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash && CheckFuel(dashFuelCost))
        {
            StartCoroutine(Dash());
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

}
 
