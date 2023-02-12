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

    [Tooltip("The players current ability")]
    [SerializeField] Ability.Type currentAbility;

    [Tooltip("Spot where the consumption particles go")]
    [SerializeField] ParticleSystemForceField field;

    public int abilityLevel = 0;

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
                    if (enemy.GetComponent<BaseDeadEnemy>().GetAbilityType() == currentAbility)
                    {
                        LevelUpRequest = true;
                    }
                    currentAbility = enemy.GetComponent<BaseDeadEnemy>().Consumed();
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

    public void SetCurrentAbility(Ability.Type ability)
    {
        currentAbility = ability;
    }
}
