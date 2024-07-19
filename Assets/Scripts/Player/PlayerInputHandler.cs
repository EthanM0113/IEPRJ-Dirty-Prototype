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
    [SerializeField] private KeyCode skillSlotMenu;
    [SerializeField] private KeyCode cycleSkill;
    [SerializeField] private KeyCode cycleSkill2;

    bool usingAttack = false;
    bool usingConsume = false;
    bool usingAbility = false;
    bool usingDash = false;
    bool usingSneak = false;
    bool usingSkillSlotMenu = false;
    bool usingCycle = false;

    private bool canInput = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canInput)
        {
            // Attacking
            if (Input.GetKeyDown(attack) || Input.GetKeyDown(KeyCode.U))
            {
                usingAttack = true;
            }
            else if (!Input.GetKeyDown(attack))
            {
                usingAttack = false;
            }

            // Consuming
            if (Input.GetKey(consume) || Input.GetKeyDown(KeyCode.J))
            {
                usingConsume = true;
            }
            else if (!Input.GetKey(consume))
            {
                usingConsume = false;
            }

            // Using Ability
            if (Input.GetKeyDown(ability) || Input.GetKeyDown(KeyCode.I))
            {
                usingAbility = true;
            }
            else if (!Input.GetKeyDown(ability))
            {
                usingAbility = false;
            }

            // Dashing
            if (Input.GetKeyDown(dash) || Input.GetKeyDown(KeyCode.Space))
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

            usingSkillSlotMenu = Input.GetKeyDown(skillSlotMenu) || Input.GetKeyDown(KeyCode.Tab);
            usingCycle = Input.GetKeyDown(cycleSkill) || Input.GetKeyDown(cycleSkill2) || Input.GetKeyDown(KeyCode.E);
        }  
    }

    public bool IsAttack() { return usingAttack; }
    public bool IsConsume() { return usingConsume; }
    public bool IsAbility() { return usingAbility; }
    public bool IsDash() { return usingDash; }
    public bool IsSneak() { return usingSneak; }

    public bool IsSkillSlotMenuActive() { return usingSkillSlotMenu; }
    public bool IsCycle() { return usingCycle; }

    public void SetCanInput(bool flag) { canInput = flag; }
}
