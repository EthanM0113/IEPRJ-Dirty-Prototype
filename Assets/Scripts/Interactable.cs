using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject lightSource;
    bool isInteractable = false;

    // Fuel Varaibles
    [SerializeField] private float fuelCost = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        lightSource.SetActive(false);
        isInteractable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !isInteractable)
        {
            if (other.gameObject.GetComponent<PlayerController>().GetFuel() > 0.0f)
            {
                lightSource.SetActive(true);

                PlayerController playerController = other.GetComponent<PlayerController>();
                playerController.fuelAmt -= fuelCost;
                isInteractable = true;
            }
            
        }
    }


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.CompareTag("Player")) {
    //        //Debug.Log("This works?");
    //        lightSource.SetActive(false);
    //    }
    //}
}
