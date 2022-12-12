using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarHandler : MonoBehaviour
{
    public Slider fuelMeter;

    private float fuelMax;

    PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        fuelMax = playerController.MAX_FUEL;
    }

    void Update()
    {
        updateFuel();
    }

    void updateFuel()
    {
        fuelMeter.value = playerController.fuelAmt;
    }

    public void resetFuel(int amt)
    {
        Debug.Log("Nerfed fuel");
        // Change max fuel by integer amt
        fuelMax -= amt;
        playerController.fuelAmt = fuelMax;
    }

    
}
