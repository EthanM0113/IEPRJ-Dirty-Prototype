using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCloset : MonoBehaviour
{
    [SerializeField] private GameObject playerInventory;

    public void OpenCloset()
    {
        if (playerInventory.GetComponent<PlayerInventory>().GetKey())
        {
            // TODO: display got chalice text
            playerInventory.GetComponent<PlayerInventory>().SetChalice(true);
        }
        else
        {
            // TODO: display need key text
        }
    }
}
