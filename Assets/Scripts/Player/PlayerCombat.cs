using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayers;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputListener();
    }

    void InputListener()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Attack();
        }  
    }

    void Attack()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position, 
            attackRange, 
            enemyLayers
            );

        // inflict dmg/kill
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<FaceDirection>().GetFaceDirection() != playerController.GetFaceDirection()) // if the enemy is facing the same direction
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
