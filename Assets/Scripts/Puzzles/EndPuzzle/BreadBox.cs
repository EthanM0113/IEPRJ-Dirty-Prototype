using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBox : MonoBehaviour
{
    [SerializeField] private GameObject playerInv;
    [SerializeField] private GameObject box;

    private int breadNum;
    private bool keyObtained;

    void Awake()
    {
        breadNum = 0;
        keyObtained = false;
    }

    public void TakeBread()
    {
        if (breadNum >= 4)
        {
            if (keyObtained)
                return;

            playerInv.GetComponent<PlayerInventory>().SetKey(true);
            box.GetComponent<LoreObject>().UpdateText("Obtained a rusty key!");
            keyObtained = true;
        }
        else
        {
            breadNum++;
            playerInv.GetComponent<PlayerInventory>().SetBread(breadNum);

            Debug.Log("Bread num: " + breadNum);

            switch (breadNum)
            {
                case 1:
                    box.GetComponent<LoreObject>().UpdateText("Got some moldy bread! Hm. . . there's more.");
                    break;
                case 2:
                    box.GetComponent<LoreObject>().UpdateText("Ah, more moldy bread. Why not?");
                    break;
                case 3:
                    box.GetComponent<LoreObject>().UpdateText("Surely one more wouldn't hurt?");
                    break;
                case 4:
                    box.GetComponent<LoreObject>().UpdateText("Huh? There's something rusty amidst the bread.");
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (breadNum >= 4)
            {
                if (keyObtained)
                {
                    box.GetComponent<LoreObject>().SetText("Nothing but moldy bread now, there's no need for more.");
                }
                else
                    box.GetComponent<LoreObject>().SetText("Huh? There's something rusty amidst the bread.");
            }
            else
            {
                switch (breadNum)
                {
                    case 1:
                        box.GetComponent<LoreObject>().SetText("Got some moldy bread! Hm. . . there's more.");
                        break;
                    case 2:
                        box.GetComponent<LoreObject>().SetText("Ah, more moldy bread. Why not?");
                        break;
                    case 3:
                        box.GetComponent<LoreObject>().SetText("Surely one more wouldn't hurt?");
                        break;
                }
            }
        }
    }
}
