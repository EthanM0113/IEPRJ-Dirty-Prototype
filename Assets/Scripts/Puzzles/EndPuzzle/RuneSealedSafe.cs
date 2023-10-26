using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneSealedSafe : MonoBehaviour
{
    [SerializeField] private GameObject colorPuzzleMaster;
    [SerializeField] private GameObject sealedSafe;
    [SerializeField] private GameObject playerInventory;

    private bool unlocked = false;
    private bool obtained = false;

    public void Interact()
    {

        if(CheckSolved() && !unlocked)
        {
            sealedSafe.GetComponent<Renderer>().material.color = Color.yellow;
            sealedSafe.GetComponent<LoreObject>().UpdateText("The seal is broken! Looks like there's a gear in here.");
            unlocked = true;
        }
        else if(unlocked && !obtained)
        {
            playerInventory.GetComponent<PlayerInventory>().SetGear(true);
            sealedSafe.GetComponent<LoreObject>().UpdateText("Obtained a Gear! Oof, it's heavy.");
            obtained = true;

            PlayerMoneyManager.Instance.AddCoins(30);
        }
        else if (!unlocked)
        {
            sealedSafe.GetComponent<LoreObject>().UpdateText("The seal isn't budging yet. . .");
        }
    }

    private bool CheckSolved()
    {
        return colorPuzzleMaster.GetComponent<ColorTorchPuzzle>().GetSolved();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (CheckSolved())
                sealedSafe.GetComponent<LoreObject>().SetText("The Runic seal has been weakened. A good smack oughta do it.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(obtained)
                sealedSafe.GetComponent<LoreObject>().SetText("It's emptied out.");
        }
    }
}