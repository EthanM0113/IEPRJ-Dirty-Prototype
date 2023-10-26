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

    void Update()
    {
        if (unlocked)
            return;

        if (CheckSolved())
        {
            // TODO: palce text of get gear.
            playerInventory.GetComponent<PlayerInventory>().SetGear(true);
            // TODO: give player money.
            sealedSafe.GetComponent<Renderer>().material.color = Color.yellow;
            sealedSafe.GetComponent<LoreObject>().SetText("the seal is broken! This safe has the gear for the door!");
            unlocked = true;
        }
    }

    private bool CheckSolved()
    {
        return colorPuzzleMaster.GetComponent<ColorTorchPuzzle>().GetSolved();
    }
}