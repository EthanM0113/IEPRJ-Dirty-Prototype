using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandDoor : MonoBehaviour
{
    [SerializeField] private GameObject grandDoor;
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private Animator grandDoorAnimator;

    private bool doorFixed = false;

    public void Interact()
    {
        if (playerInventory.GetComponent<PlayerInventory>().GetGear())
        {
            if (doorFixed)
                return;

            playerInventory.GetComponent<PlayerInventory>().SetGear(false);
            grandDoor.GetComponent<LoreObject>().UpdateText("The gear slides perfectly into place. The grand door is fixed!");
            doorFixed = true;
            grandDoorAnimator.SetBool("isSolved", true);
            return;
        }
        else
        {
            if(doorFixed) 
                return;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!doorFixed && playerInventory.GetComponent<PlayerInventory>().GetGear())
                grandDoor.GetComponent<LoreObject>().UpdateText("Looks like the gear can fit in this door's mechanism.");
        }
    }
}
