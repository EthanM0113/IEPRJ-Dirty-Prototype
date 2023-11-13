using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTorchHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // On collision with player, refill fuel
        if (other.transform.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.fuelAmt = playerController.GetMaxFuel();
        }
    }
}
