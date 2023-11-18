using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCloset : MonoBehaviour
{
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject closet;
    private bool unlocked = false;
    private bool obtained = false;

    public void OpenCloset()
    {
        if (!unlocked)
        {
            if (playerInventory.GetComponent<PlayerInventory>().GetKey())
            {
                closet.GetComponent<LoreObject>().UpdateText("The rusted key fit right in! There's a chalice inside.");
                playerInventory.GetComponent<PlayerInventory>().SetKey(false);
                unlocked = true;
            }
            else
            {
                closet.GetComponent<LoreObject>().UpdateText("It's shut real tight.");
            }
        }
        else if (unlocked && !obtained)
        {
            playerInventory.GetComponent<PlayerInventory>().SetChalice(true);
            closet.GetComponent<LoreObject>().UpdateText("Obtained the chalice!");
            obtained = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (playerInventory.GetComponent<PlayerInventory>().GetChalice())
        {
            if(other.gameObject.tag == "Player")
            {
                closet.GetComponent<LoreObject>().SetText("I already got the chalice from here. It's empty now.");
            }
        }
    }
}
