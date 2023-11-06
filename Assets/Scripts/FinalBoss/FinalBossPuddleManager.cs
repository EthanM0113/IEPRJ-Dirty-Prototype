using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossPuddleManager : MonoBehaviour
{
    PlayerHearts playerHearts;
    private bool didDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        playerHearts = FindObjectOfType<PlayerHearts>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!didDamage)
            {
                playerHearts.DamagePlayer(2); // 2 Damage
                didDamage = true;
                Debug.Log("PUDDLE DAMAGED PLAYER");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        didDamage = false;
    }
}
