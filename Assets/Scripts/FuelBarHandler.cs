using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class FuelBarHandler : MonoBehaviour
{
    public Slider fuelMeter;

    [SerializeField] private float maxFuel;

    PlayerController playerController;

    [SerializeField] private GameObject flameHandle;

    Vector3 originalFlameScale;

    [SerializeField] Animator flameAnim;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        maxFuel = playerController.GetMaxFuel();
        originalFlameScale = flameHandle.transform.localScale;
    }

    void Update()
    {
        updateFuel();
        updateFlameHandle();
    }

    void updateFuel()
    {
        fuelMeter.value = playerController.fuelAmt;
        if (fuelMeter.value <= 1f)
        {
            flameHandle.SetActive(false);
        }
        else
        {
            flameHandle.SetActive(true);
        }
    }

    void updateFlameHandle()
    {
        flameHandle.transform.localScale = (fuelMeter.value/ maxFuel)* 2 * originalFlameScale ;
        flameAnim.speed = (fuelMeter.value / maxFuel);
    }

    public void ResetFuel(float amt)
    {
        Debug.Log("Nerfed fuel");
        // Change max fuel by integer amt
        maxFuel -= amt;
        playerController.fuelAmt = maxFuel;
        playerController.SetMaxFuel(maxFuel);
    }

    public void IncreaseMaxFuel(float amount)
    {
        Debug.Log("Increased Max Fuel");
        maxFuel += amount;
        fuelMeter.maxValue = maxFuel;
        playerController.fuelAmt = maxFuel;
        playerController.SetMaxFuel(maxFuel);
    }

    
}
