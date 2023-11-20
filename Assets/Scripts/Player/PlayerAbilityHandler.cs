using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class PlayerAbilityHandler : MonoBehaviour
{
    [System.Serializable]
    public class AbilityStats
    {
        public AbilityStats(Ability.Type abilityVal, int levelVal) { ability = abilityVal; level = levelVal; }
        public Ability.Type ability;
        public int level = 0;
    }

    [SerializeField] ParticleSystem levelUpParticles;
    [SerializeField] ParticleSystem newAbilityParticles;
    [SerializeField] Animator cameraAnim;

    Transform attackPoint;
    float consumeRange;
    [Tooltip("Determines what layers is the \"Dead\" enemies")]
    [SerializeField] LayerMask consumeLayerMask;

    //[Tooltip("The players previous ability")]
    //[SerializeField] Ability.Type previousAbility;

    [Tooltip("The players current ability")]
    [SerializeField] List<Ability.Type> currentAbility;

    [SerializeField] int skillSlotIndex = 0;
    [SerializeField] int skillSlotCount = 1;
    [SerializeField] int skillSlotToEdit = 1;

    [Tooltip("Spot where the consumption particles go")]
    [SerializeField] ParticleSystemForceField field;

    [SerializeField] int abilityLevel1 = 0;
    [SerializeField] int abilityLevel2 = 0;
    [SerializeField] int abilityLevel3 = 0;

    [SerializeField] List<AbilityStats> consumedAbilities = new List<AbilityStats>();

    private UISkillHandler uISkillHandler;

    // Start is called before the first frame update
    void Start()
    {
        attackPoint = GetComponent<PlayerCombat>().GetAttackTransform();
        consumeRange = GetComponent<PlayerCombat>().GetAttackRange();
        uISkillHandler = FindAnyObjectByType<UISkillHandler>();
        //currentAbility.Add(Ability.Type.NONE);
        consumedAbilities = new List<AbilityStats>(PlayerDataHolder.Instance.GetAbilitiesReference());
        currentAbility = new List<Ability.Type>(PlayerDataHolder.Instance.GetCurrentAbilities());
        skillSlotCount = PlayerDataHolder.Instance.GetSkillSlotCount();
    }

    private void Update()
    {
        uISkillHandler.SetAbility(currentAbility[skillSlotIndex]);
    }

    public void Consume()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            consumeRange,
            consumeLayerMask
            );

        //bool LevelUpRequest = false;
        // inflict dmg/kill
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<BaseDeadEnemy>().SetConsumed(true);
            enemy.GetComponent<BaseDeadEnemy>().AddInfluence(field);
            // When timer in the dead enemy is done. Refer to DeadTestEnemy Consumed()
            if (enemy.GetComponent<BaseDeadEnemy>().Consumed() != Ability.Type.NONE) 
            {
                if (enemy.gameObject.CompareTag("DeadTestEnemy"))
                {
                    //if (enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() != previousAbility && previousAbility != Ability.Type.NONE)
                    //{
                    //    LevelUpRequest = false;
                    //    abilityLevel = 0;
                    //}
                    //else if (enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() == currentAbility || enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() == previousAbility)
                    //{
                    //    LevelUpRequest = true;
                    //    previousAbility = Ability.Type.NONE;

                    //}


                    //currentAbility = enemy.GetComponent<BaseDeadEnemy>().Consumed();

                    Ability.Type consumedAbility = enemy.GetComponent<BaseDeadEnemy>().Consumed();

                    // Checks if tghe ability has been consumed
                    int flag = FindAbility(consumedAbility);

                    if (flag != -1)
                    {
                        consumedAbilities[flag].level++;
                        if (levelUpParticles != null)
                        {
                            levelUpParticles.Play();
                            //cameraAnim.SetTrigger("isQuickZoom");
                        }
                    }
                    else
                    {
                        if (consumedAbilities.Count == 0)
                        {
                            currentAbility[skillSlotIndex] = consumedAbility;
                        }
                        consumedAbilities.Add(new AbilityStats(consumedAbility, 0));
                        if (newAbilityParticles != null) 
                        { 
                            newAbilityParticles.Play();
                            cameraAnim.SetTrigger("isQuickZoom");
                        }
                    }
                    //previousAbility = currentAbility;


                }
            }
        }
        //if (LevelUpRequest)
        //{
        //    abilityLevel++;
        //}
    }

    public void Unconsume()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            consumeRange,
            consumeLayerMask
            );

        // inflict dmg/kill
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<BaseDeadEnemy>().SetConsumed(false);
        }
    }

    public Ability.Type GetCurrentAbility()
    {
        return currentAbility[skillSlotIndex];
    }

    public List<Ability.Type> GetCurrentAbilities()
    {
        return currentAbility;
    }

    public int GetAbilityLevel()
    {
        if (consumedAbilities.Count == 0)
        {
            return -1;
        }
        return consumedAbilities[FindAbility(currentAbility[skillSlotIndex])].level;
    }
    //public void SetCurrentAbility(Ability.Type ability)
    //{
    //    currentAbility = ability;
    //}

    public int SetCurrentAbility(Ability.Type ability)
    {
        if (skillSlotToEdit + 1 > skillSlotCount)
        {
            skillSlotToEdit = 1;
        }
        else
        {
            skillSlotToEdit++;
        }
        Debug.Log("skillSlotToEdit " + skillSlotToEdit);
        if (FindAbility(ability) != -1)
        {
            if (skillSlotToEdit == 1)
            {
                currentAbility[0] = ability;
                abilityLevel1 = consumedAbilities[FindAbility(ability)].level;
            }
            else if (skillSlotToEdit == 2)
            {
                currentAbility[1] = ability;
                abilityLevel2 = consumedAbilities[FindAbility(ability)].level;
            }
            else if (skillSlotToEdit == 3)
            {
                currentAbility[2] = ability;
                abilityLevel3 = consumedAbilities[FindAbility(ability)].level;
            }

        }
        return skillSlotToEdit;
    }

    public List<AbilityStats> GetConsumedAbilities()
    {
        return consumedAbilities;
    }

    // returns the index
    int FindAbility(Ability.Type ability)
    {
        int flag = -1;
        for (int i = 0; i < consumedAbilities.Count; i++)
        {
            if (consumedAbilities[i].ability == ability)
            {
                flag = i; break;
            }
        }
        
        return flag;
    }

    public void AddSkillSlot()
    {
        skillSlotCount++;
        currentAbility.Add(Ability.Type.NONE);
    }

    public int GetSkillSlotCount()
    {
        return skillSlotCount;
    }

    public void SelectSkillSlot(int index)
    {
        skillSlotToEdit = index;
    }

    public void CycleSkills()
    {
        if (skillSlotIndex + 1 < skillSlotCount)
        {
            skillSlotIndex += 1;

        }
        else
        {
            skillSlotIndex = 0;
        }
    }
}


