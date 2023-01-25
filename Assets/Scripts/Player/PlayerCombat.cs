using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCombat : MonoBehaviour
{
    [Tooltip("Position of the attack")]
    [SerializeField] Transform attackPoint;
    [Tooltip("The Radius on which the attack with have effect on")]
    [SerializeField] float attackRange;
    [Tooltip("The layers that are considered as enemies")]
    [SerializeField] LayerMask enemyLayers;
    PlayerController playerController;

    // 1st boss stuff
    [SerializeField] private string hpTorchTag; 

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Attack()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position, 
            attackRange, 
            enemyLayers
            );

        // inflict dmg/kill
        if (hitEnemies != null)
        {
            foreach (Collider enemy in hitEnemies)
            {
                // 1st Boss Stuff
                FirstBoss(enemy);

                if (enemy.GetComponent<FaceDirection>().GetFaceDirection() == playerController.GetFaceDirection()) // if the enemy is facing the same direction
                {
                    enemy.GetComponent<BaseEnemy>().EnemyDeath();
                    enemy.gameObject.SetActive(false);
                }
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Returns the attack position
    public Transform GetAttackTransform() { return attackPoint; }

    // Returns the attack range
    public float GetAttackRange() { return (attackRange); }

    public void FirstBoss(Collider enemy)
    {
        if(enemy.CompareTag(hpTorchTag))
        {
            enemy.gameObject.GetComponent<HpTorchHandler>().SetFlameLight(false);
        }
    }
}
