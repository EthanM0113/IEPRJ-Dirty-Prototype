using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject playerInv;
    
    // 0 - normal, 1 - hot, 2 - cold
    private int state;

    void Awake()
    {
        state = 0;
    }

    public void Interact()
    {
        switch(state)
        {
            case 0:
                if (playerInv.GetComponent<PlayerInventory>().GetWater())
                    barrel.GetComponent<LoreObject>().UpdateText("Nothing happened. . .");
                break;
            case 1:
                if (playerInv.GetComponent<PlayerInventory>().GetWater())
                {
                    barrel.GetComponent<LoreObject>().UpdateText("The water sizzled up and evaporated. . .");
                    playerInv.GetComponent<PlayerInventory>().SetWater(false);
                }
                break;
            case 2:
                if (playerInv.GetComponent<PlayerInventory>().GetWater())
                {
                    barrel.GetComponent<LoreObject>().UpdateText("The water froze! Obtained chunks of ice.");
                    playerInv.GetComponent<PlayerInventory>().SetWater(false);
                    playerInv.GetComponent<PlayerInventory>().SetIceChunks(true);
                }
                break;

        }
    }

    public void SwitchState()
    {
        state++;

        if (state > 2)
            state = 0;

        switch (state)
        {
            case 0:
                barrel.GetComponent<LoreObject>().SetText("It looks mechanical, there must be a trigger somewhere.");
                barrel.GetComponent<Renderer>().material.color = Color.gray;
                break;
            case 1:
                barrel.GetComponent<LoreObject>().SetText("It's red hot!");
                barrel.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                barrel.GetComponent<LoreObject>().SetText("Cold to the touch. It can even freeze water.");
                barrel.GetComponent<Renderer>().material.color = Color.cyan;
                break;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            switch (state)
            {
                case 0:
                    barrel.GetComponent<LoreObject>().SetText("It looks mechanical, there must be a trigger somewhere.");
                    barrel.GetComponent<Renderer>().material.color = Color.gray;
                    break;
                case 1:
                    barrel.GetComponent<LoreObject>().SetText("It's red hot!");
                    barrel.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 2:
                    barrel.GetComponent<LoreObject>().SetText("Cold to the touch. It can even freeze water.");
                    barrel.GetComponent<Renderer>().material.color = Color.cyan;
                    break;

            }
        }
    }
}
