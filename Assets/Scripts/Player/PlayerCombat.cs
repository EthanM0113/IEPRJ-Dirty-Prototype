using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private Camera mainCamera;
    private Animator mainCameraAnimator;

    // Coin stuff
    private int killReward;
    [SerializeField] PlayerMoneyUIHandler playerMoneyUIHandler; 

    // 1st boss stuff
    [SerializeField] private string hpTorchTag; 

    // Start is called before the first frame update
    void Start()
    {
        killReward = 0;
        playerController = GetComponent<PlayerController>();
        mainCameraAnimator = mainCamera.GetComponent<Animator>();
    }

    public bool CheckRadius()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayers
            );

        if (hitEnemies.Length == 0) return false;
        else return true;
        
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
        if (hitEnemies.Length != 0)
        {
            foreach (Collider enemy in hitEnemies)
            {
                // 1st Boss Stuff
                if (FirstBoss(enemy))
                {
                    return;
                }
                else // Normal enemy stuff
                {
                    if (enemy.GetComponent<FaceDirection>().GetFaceDirection() == playerController.GetFaceDirection()) // if the enemy is facing the same direction
                    {
                        mainCameraAnimator.SetTrigger("isQuickZoom");
                        SoundManager.Instance.BackstabHit();   
                        enemy.GetComponent<BaseEnemy>().EnemyDeath();

                        // Add coins
                        if (enemy.name.Contains("TestEnemy")) // Beholder
                        {
                            killReward = UnityEngine.Random.Range(3, 5);
                        }
                        else if (enemy.name.Contains("SampleWisp")) // Wisp
                        {
                            killReward = UnityEngine.Random.Range(6, 8);
                        }
                        playerMoneyUIHandler.PulseCointText();
                        PlayerMoneyManager.Instance.AddCoins(killReward);
                    }
                } 
            }
        }
        else
        {
            SoundManager.Instance.BackstabMiss();
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

    public bool FirstBoss(Collider enemy)
    {
        if(enemy.CompareTag(hpTorchTag))
        {
            enemy.gameObject.GetComponent<HpTorchHandler>().SetFlameLight(false);
            return true;
        }
        else if(enemy.CompareTag("ColorTorch"))
        {
            enemy.gameObject.GetComponent<ColorTorch>().SetFlameLight(false);
            return true;
        }
        else if (enemy.CompareTag("MysticStone"))
        {
            enemy.gameObject.GetComponent<MysticStone>().ChangeColor();
            return true;
        }
        else if (enemy.CompareTag("LockedCloset"))
        {
            enemy.gameObject.GetComponent<LockedCloset>().OpenCloset();
            return true;
        }
        else if (enemy.CompareTag("BreadBox"))
        {
            enemy.gameObject.GetComponent<BreadBox>().TakeBread();
            return true;
        }
        else if (enemy.CompareTag("HelpfulChair"))
        {
            enemy.gameObject.GetComponent<HelpfulChair>().TakeScroll();
            return true;
        }
        else if (enemy.CompareTag("WaterFountain"))
        {
            enemy.gameObject.GetComponent<WaterFountain>().TakeWater();
            return true;
        }
        else
            return false;
    }
}   