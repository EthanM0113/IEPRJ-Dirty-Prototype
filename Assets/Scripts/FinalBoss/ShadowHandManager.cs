using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float handTicks = 0f;
    [SerializeField] private float handDuration;
    private float damageTimePeriod = 1f;
    private bool isPlayerColliding = false;
    private bool didDamage = false;
    PlayerHearts playerHearts;

    // Start is called before the first frame update
    void Start()
    {
        playerHearts = FindObjectOfType<PlayerHearts>();
    }

    // Update is called once per frame
    void Update()
    {
        handTicks += Time.deltaTime;
        if (handTicks > handDuration)
        {
            Destroy(gameObject);
        }
        else if (handTicks > damageTimePeriod)
        {
            TriggerDamage();
        }


    }

    public void TriggerDamage()
    {
        if(!didDamage)
        {
            // Damage Player
            if (isPlayerColliding)
            {
                playerHearts.DamagePlayer(1, true);
            }

            didDamage = true;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false;
        }
    }
}
