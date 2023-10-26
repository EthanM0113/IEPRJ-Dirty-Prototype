using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandDoor : MonoBehaviour
{
    [SerializeField] private GameObject grandDoor;
    [SerializeField] private GameObject playerInventory;

    private bool doorFixed = false;

    public void Interact()
    {
        if (playerInventory.GetComponent<PlayerInventory>().GetGear())
        {
            playerInventory.GetComponent<PlayerInventory>().SetGear(false);
            grandDoor.GetComponent<LoreObject>().UpdateText("The gear slides perfectly into place. The grand door is fixed!");
            doorFixed = true;
            // TODO: script to transport player to the next level.
            return;
        }
        else
        {
            grandDoor.GetComponent<LoreObject>().UpdateText("It needs some kind of gear to crank open.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!doorFixed)
                grandDoor.GetComponent<LoreObject>().SetText("This is definitely the door to the next floor up.");
        }
    }
}
