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

        // inflict dmg/kill
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<DeadTestEnemy>().SetConsumed(true);
            enemy.GetComponent<DeadTestEnemy>().AddInfluence(field);
            // When timer in the dead enemy is done. Refer to DeadTestEnemy Consumed()
            if (enemy.GetComponent<DeadTestEnemy>().Consumed() != Ability.Type.NONE) 
            {
                if (enemy.gameObject.CompareTag("DeadTestEnemy"))
                {
                    currentAbility = enemy.GetComponent<DeadTestEnemy>().Consumed();
                } 
            }
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
