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

    Rigidbody rb;

    bool isSneaking;

    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        isSneaking = false;
        actualSpeed = speed;
    }

    private void Update()
    {
        InputHandler();
        ChooseSpeed();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    
    void InputHandler() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            isSneaking = !isSneaking;
        }
    }

    void ChooseSpeed() {
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
    }
}
 
