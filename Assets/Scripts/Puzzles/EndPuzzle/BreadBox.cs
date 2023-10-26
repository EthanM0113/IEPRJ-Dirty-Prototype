using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBox : MonoBehaviour
{
    [SerializeField] private GameObject playerInv;
    [SerializeField] private GameObject box;

    private int breadNum;

    void Awake()
    {
        breadNum = 0;
    }

    public void TakeBread()
    {
        if (breadNum >= 4)
        {
            playerInv.GetComponent<PlayerInventory>().SetKey(true);
            box.GetComponent<LoreObject>().UpdateText("Obtained a rusty key!");
        }
        else
        {
            box.GetComponent<LoreObject>().UpdateText("Got some moldy bread!");
            breadNum++;
            playerInv.GetComponent<PlayerInventory>().SetBread(breadNum);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (breadNum >= 4)
            {
                box.GetComponent<LoreObject>().SetText("There's something rusty amidst the bread.");
            }
            else
            {
                box.GetComponent<LoreObject>().SetText("A box full of moldy bread. . .");
            }
        }
    }
}
