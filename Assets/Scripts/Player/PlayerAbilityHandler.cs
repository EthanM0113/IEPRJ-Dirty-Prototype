using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Transform attackPoint;
    float consumeRange;
    [Tooltip("Determines what layers is the \"Dead\" enemies")]
    [SerializeField] LayerMask consumeLayerMask;

    //[Tooltip("The players previous ability")]
    //[SerializeField] Ability.Type previousAbility;

    [Tooltip("The players current ability")]
    [SerializeField] Ability.Type currentAbility = Ability.Type.NONE;

    [Tooltip("Spot where the consumption particles go")]
    [SerializeField] ParticleSystemForceField field;

    [SerializeField] int abilityLevel = 0;

    [SerializeField] List<AbilityStats> consumedAbilities = new List<AbilityStats>();

    private UISkillHandler uISkillHandler;

    // Start is called before the first frame update
    void Start()
    {
        attackPoint = GetComponent<PlayerCombat>().GetAttackTransform();
        consumeRange = GetComponent<PlayerCombat>().GetAttackRange();
        uISkillHandler = FindAnyObjectByType<UISkillHandler>();
    }

    private void Update()
    {
        uISkillHandler.SetAbility(currentAbility);
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
                    }
                    else
                    {
                        if (consumedAbilities.Count == 0)
                        {
                            currentAbility = consumedAbility;
                        }
                        consumedAbilities.Add(new AbilityStats(consumedAbility, 0));
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
        return currentAbility;
    }

    public int GetAbilityLevel()
    {
        if (consumedAbilities.Count == 0)
        {
            return -1;
        }
        return consumedAbilities[FindAbility(currentAbility)].level;
    }
    //public void SetCurrentAbility(Ability.Type ability)
    //{
    //    currentAbility = ability;
    //}

    public void SetCurrentAbility(Ability.Type ability)
    {
        if (FindAbility(ability) != -1)
        {
            currentAbility = ability;
            abilityLevel = consumedAbilities[FindAbility(ability)].level;
        }
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
}


