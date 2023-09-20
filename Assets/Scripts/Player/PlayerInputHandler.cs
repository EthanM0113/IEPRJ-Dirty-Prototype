using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode attack;
    [SerializeField] private KeyCode consume;
    [SerializeField] private KeyCode ability;
    [SerializeField] private KeyCode dash;
    [SerializeField] private KeyCode sneak;

    bool usingAttack = false;
    bool usingConsume = false;
    bool usingAbility = false;
    bool usingDash = false;
    bool usingSneak = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Attacking
        if (Input.GetKeyDown(attack))
        {
            usingAttack = true;
        }
        else if (!Input.GetKeyDown(attack))
        {
            usingAttack = false;
        }

        // Consuming
        if (Input.GetKey(consume))
        {
            usingConsume = true;
        }
        else if (!Input.GetKey(consume))
        {
            usingConsume = false;
        }

        // Using Ability
        if (Input.GetKeyDown(ability))
        {
            usingAbility = true;
        }
        else if (!Input.GetKeyDown(ability))
        {
            usingAbility = false;
        }

        // Dashing
        if (Input.GetKeyDown(dash))
        {
            usingDash = true;
        }
        else if (!Input.GetKeyDown(dash))
        {
            usingDash = false;
        }

        // Sneaking
        if (Input.GetKey(sneak))
        {
            usingSneak = true;
        }
        else if (!Input.GetKey(sneak))
        {
            usingSneak = false;
        }
   
    }

    public bool IsAttack() { return usingAttack; }
    public bool IsConsume() { return usingConsume; }
    public bool IsAbility() { return usingAbility; }
    public bool IsDash() { return usingDash; }
    public bool IsSneak() { return usingSneak; }
}
