using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class PlayerCombat : MonoBehaviour
{
    [Tooltip("Position of the attack")]
    [SerializeField] Transform attackPoint;
    [Tooltip("The Radius on which the attack with have effect on")]
    [SerializeField] float attackRange;
    [Tooltip("The layers that are considered as enemies")]
    [SerializeField] LayerMask enemyLayers;
    [Tooltip("The layers that are considered as puzzle elements")]
    [SerializeField] LayerMask puzzleElementLayer;
    PlayerController playerController;

    [SerializeField] private Camera mainCamera;
    private Animator mainCameraAnimator;

    // Coin stuff
    private int killReward;
    [SerializeField] PlayerMoneyUIHandler playerMoneyUIHandler; 

    // 1st boss stuff
    [SerializeField] private string hpTorchTag;

    // Final boss stuff
    [SerializeField] private string finalBossTag;

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

        if (hitEnemies.Length == 0)
        {
            hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            puzzleElementLayer
            );
        }

        if (hitEnemies.Length == 0)
            return false;
        else
        {
            return true;
        }
        
    }

    public void Attack()
    {
        // Gets all of the enemies within the hit Collider
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position, 
            attackRange, 
            enemyLayers
            );

        if (hitEnemies.Length == 0)
        { 
            hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            puzzleElementLayer
            );
        }

        // inflict dmg/kill
        if (hitEnemies.Length != 0)
        {
            foreach (Collider enemy in hitEnemies)
            {
                if (FirstBoss(enemy)) // 1st Boss Stuff
                {
                    Debug.Log("Enemy 1");
                    return;
                }
                
                else if(FinalBoss(enemy))
                {
                    Debug.Log("Enemy 2");
                    return;
                }
                else // Normal enemy stuff
                {

                    Debug.Log("Enemy 3");
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
        Debug.Log("enemy: " + enemy.gameObject.tag);
        if(enemy.CompareTag(hpTorchTag))
        {
            SoundManager.Instance.TB_ExtinguishTorch();
            enemy.gameObject.GetComponent<HpTorchHandler>().SetFlameLight(false);
            return true;
        }
        else if(enemy.gameObject.tag == "ColorTorch")
        {
            enemy.gameObject.GetComponent<ColorTorch>().SetFlameLight(true);
            Debug.Log("change color");
            return true;
        }
        else if (enemy.gameObject.tag == "MysticStone")
        {
            enemy.gameObject.GetComponent<MysticStone>().ChangeColor();
            return true;
        }
        else if (enemy.gameObject.tag == "LockedCloset")
        {
            enemy.gameObject.GetComponent<LockedCloset>().OpenCloset();
            return true;
        }
        else if (enemy.gameObject.tag == "BreadBox")
        {
            enemy.gameObject.GetComponent<BreadBox>().TakeBread();
            return true;
        }
        else if (enemy.gameObject.tag == "HelpfulChair")
        {
            enemy.gameObject.GetComponent<HelpfulChair>().TakeScroll();
            return true;
        }
        else if (enemy.gameObject.tag == "WaterFountain")
        {
            enemy.gameObject.GetComponent<WaterFountain>().TakeWater();
            return true;
        }
        else if (enemy.gameObject.tag == "HotRock")
        {
            enemy.gameObject.GetComponent<HotRock>().Interact();
            return true;
        }
        else if (enemy.gameObject.tag == "WetRock")
        {
            enemy.gameObject.GetComponent<WetRock>().Interact();
            return true;
        }
        else if (enemy.gameObject.tag == "Barrel")
        {
            enemy.gameObject.GetComponent<Barrel>().Interact();
            return true;
        }
        else if (enemy.gameObject.tag == "GrandDoor")
        {
            enemy.gameObject.GetComponent<GrandDoor>().Interact();
            return true;
        }
        else if (enemy.gameObject.tag == "RuneSealedSafe")
        {
            enemy.gameObject.GetComponent<RuneSealedSafe>().Interact();
            return true;
        }
        else
            return false;
    }

    public bool FinalBoss(Collider enemy)
    {
        if (enemy.CompareTag(finalBossTag))
        {
            FinalBossManager finalBossManager = FindObjectOfType<FinalBossManager>();
            if(finalBossManager != null) 
            {
                StartCoroutine(finalBossManager.DamageBoss());
            }
          
            return true;
        }
        else
            return false;
    }
}   