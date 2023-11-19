using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBOrbManager : MonoBehaviour
{
    private float ticks = 0f;
    private float lifetime = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ticks += Time.deltaTime;
        if(ticks > lifetime) 
        { 
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerHearts playerHearts = FindObjectOfType<PlayerHearts>();
            playerHearts.DamagePlayer(1, false);
        }
    }
}
