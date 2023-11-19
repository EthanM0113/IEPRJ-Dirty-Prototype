using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAttack : MonoBehaviour
{

    [SerializeField] private Animator bossAnimator;
    //[SerializeField] private GameObject respawnPoint;
    [SerializeField] private FirstBossManager firstBossManager;
    [SerializeField] private FirstBossAnimationHandler firstBossAnimationHandler;
    [SerializeField] private GameObject player;
    private PlayerHearts playerHearts;
    FuelBarHandler fuelBarHandler;
    private Animator playerAnimator;
    private PlayerController playerController;
    private float attackdelayTicks = 0f;
    private float attackdelayInterval = 2.5f;
    private bool didAttack = false;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
       // respawnPoint = GameObject.FindGameObjectWithTag("RespawnNode");
        player = GameObject.FindGameObjectWithTag("Player");
        playerHearts = GameObject.FindObjectOfType<PlayerHearts>();
        fuelBarHandler = GameObject.FindObjectOfType<FuelBarHandler>();
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>(); 


    }

    // Update is called once per frame
    void Update()
    { 
        if (didAttack)
        {
            attackdelayTicks += Time.deltaTime;
            if(attackdelayTicks > attackdelayInterval) 
            {
                canAttack = true;
                didAttack = false;
                attackdelayTicks = 0f;
            }
        }


        if (firstBossAnimationHandler.isAttacking)
        {
            Debug.Log("Attacking!");
        }
    
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if player is sneaking
            if (!other.GetComponent<PlayerController>().GetSneaking())
            {
                StartCoroutine(BossAttack());
            }
        }
    }
    */

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(canAttack)
                StartCoroutine(BossAttack());       
        }
    }

    public IEnumerator BossAttack()
    {
        canAttack = false;
        didAttack = true;
        bossAnimator.SetTrigger("isAttacking");
        yield return new WaitForSecondsRealtime(0.80f); // Fine tune to fill in the animation 
        Debug.Log("Player Hit!");
        // Damage Player
        //DamagePlayer();
        playerHearts.DamagePlayer(2, false);
    }

    /*
    public void ResetBossAndPlayer()
    {
        // Telport player to respawn point
        //player.transform.position = respawnPoint.transform.position;
        // Reset boss hp
        firstBossManager.ResetValues();
    }
    */
}
