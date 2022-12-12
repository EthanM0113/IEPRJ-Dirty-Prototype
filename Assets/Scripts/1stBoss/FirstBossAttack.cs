using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAttack : MonoBehaviour
{

    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private FirstBossManager firstBossManager;
    [SerializeField] private FirstBossAnimationHandler firstBossAnimationHandler;
    [SerializeField] private GameObject player;
    //[SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerHearts playerHearts;

    // attacking
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking)
        {
            if (firstBossAnimationHandler.isAttacking)
            {
                Debug.Log("Attacking!");
            }
            else
            {
                ResetBossAndPlayer();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if player is sneaking
            if (!other.GetComponent<PlayerController>().GetSneaking())
            {
                BossAttack();
            }
        }
    }

    public void BossAttack()
    {
        bossAnimator.SetTrigger("isAttacking");
        Debug.Log("Player Hit!");
        // Damage player
        playerHearts.DamagePlayer(1);
        attacking = true;
    }

    public void ResetBossAndPlayer()
    {
        // Telport player to respawn point
        player.transform.position = respawnPoint.transform.position;
        // Reset boss hp
        firstBossManager.ResetValues();
    }

}
