using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSealedSafe : MonoBehaviour
{
    [SerializeField] private GameObject colorPuzzleMaster;
    [SerializeField] private GameObject sealedSafe;

    void Update()
    {
        if (CheckSolved())
        {
            // TODO: give player gear.
            // TOD: give player money.
            sealedSafe.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private bool CheckSolved()
    {
        return colorPuzzleMaster.GetComponent<ColorTorchPuzzle>().GetSolved();
    }
}
