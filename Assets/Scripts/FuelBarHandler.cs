using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarHandler : MonoBehaviour
{
    public Slider fuelMeter;

    private float fuelMax;

    public PlayerController playerController;

    void Start()
    {
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
}
