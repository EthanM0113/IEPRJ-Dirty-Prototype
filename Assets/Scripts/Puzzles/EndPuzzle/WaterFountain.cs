using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFountain : MonoBehaviour
{
    [SerializeField] private GameObject fountain;
    [SerializeField] private GameObject playerInventory;

    private bool obtained = false;

    public void TakeWater()
    {
        if (playerInventory.GetComponent<PlayerInventory>().GetWater())
        {
            fountain.GetComponent<LoreObject>().UpdateText("Ah, the chalice is already full.");
            return;
        }

        if (playerInventory.GetComponent<PlayerInventory>().GetChalice())
        {
            playerInventory.GetComponent<PlayerInventory>().SetWater(true);
            fountain.GetComponent<LoreObject>().UpdateText("Scooped up some water with the chalice!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            fountain.GetComponent<LoreObject>().SetText("A fountain, miraculously still trickling some water.");
    }
}
