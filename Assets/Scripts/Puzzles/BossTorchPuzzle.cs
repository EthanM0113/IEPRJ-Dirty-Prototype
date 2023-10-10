using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTorchPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] torches;
    [SerializeField] private GameObject[] lights;

    private bool solved = false;
    private bool rewardIsGranted = false;
    private int number = 0;

    void Start()
    {
        for(int i = 0; i < torches.Length; i++)
            torches[i].GetComponent<HpTorchHandler>().SetFlameLight(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!solved)
            CheckTorches();

        if (solved && !rewardIsGranted)
        {
            for (int i = 0; i < torches.Length; i++)
                torches[i].GetComponent<HpTorchHandler>().SetFlameLight(true);

            Invoke("PuzzleSolved", 2);

            //PlayerMoneyManager.Instance.AddCoins(10);
            rewardIsGranted = true;
        }

    }

    void CheckTorches()
    {
        for(int i = 0; i < torches.Length; i++)
        {
            if (!torches[i].GetComponent<HpTorchHandler>().GetFlameLight().activeSelf)
                solved = true;
            else
            {
                solved = false;
                return;
            }
        }
    }

    void PuzzleSolved()
    {
        if (number > 5)
        {
            Invoke("FinalColor", 0.5f);
            return;
        }
        else
        {
            lights[number].GetComponent<Light>().color = Color.gray;
            number++;
            Invoke("PuzzleSolved", 0.1f);
        }
    }

    void FinalColor()
    {
        for (int i = 0; i < 6; i++)
        {
            lights[i].GetComponent<Light>().color = Color.red;
        }
    }
}
