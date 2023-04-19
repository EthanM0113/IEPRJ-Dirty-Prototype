using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAbilityHandler : MonoBehaviour
{
    Transform attackPoint;
    float consumeRange;
    [Tooltip("Determines what layers is the \"Dead\" enemies")]
    [SerializeField] LayerMask consumeLayerMask;

    [Tooltip("The players previous ability")]
    [SerializeField] Ability.Type previousAbility;

    [Tooltip("The players current ability")]
    [SerializeField] Ability.Type currentAbility;

    [Tooltip("Spot where the consumption particles go")]
    [SerializeField] ParticleSystemForceField field;

    [SerializeField] int abilityLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        attackPoint = GetComponent<PlayerCombat>().GetAttackTransform();
        consumeRange = GetComponent<PlayerCombat>().GetAttackRange();
    }

    public void Consume()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            consumeRange,
            consumeLayerMask
            );

        bool LevelUpRequest = false;
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
                    if (enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() != previousAbility && previousAbility != Ability.Type.NONE)
                    {
                        LevelUpRequest = false;
                        abilityLevel = 0;
                    }
                    else if (enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() == currentAbility || enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() == previousAbility)
                    {
                        LevelUpRequest = true;
                        previousAbility = Ability.Type.NONE;

                    }

                    currentAbility = enemy.GetComponent<BaseDeadEnemy>().Consumed();
                    previousAbility = currentAbility;


                }
            }
        }
        if (LevelUpRequest)
        {
            abilityLevel++;
        }
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
            //enemy.GetComponent<DeadTestEnemy>().SetConsumed(false);
        }
    }

    public Ability.Type GetCurrentAbility()
    {
        return currentAbility;
    }

    public int GetAbilityLevel()
    {
        return abilityLevel;
    }
    public void SetCurrentAbility(Ability.Type ability)
    {
        currentAbility = ability;
    }
}
