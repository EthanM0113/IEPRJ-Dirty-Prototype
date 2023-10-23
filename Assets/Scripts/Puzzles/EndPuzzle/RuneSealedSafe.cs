using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSealedSafe : MonoBehaviour
{
    [SerializeField] private GameObject colorPuzzleMaster;
    [SerializeField] private GameObject sealedSafe;
    [SerializeField] private GameObject playerInventory;
    
    void Update()
    {
        if (CheckSolved())
        {
            // TODO: palce text of get gear.
            playerInventory.GetComponent<PlayerInventory>().SetGear(true);
            // TODO: give player money.
            sealedSafe.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private bool CheckSolved()
    {
        return colorPuzzleMaster.GetComponent<ColorTorchPuzzle>().GetSolved();
    }
}