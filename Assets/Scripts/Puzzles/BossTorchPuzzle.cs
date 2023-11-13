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
    private int fadeNumber = 50;
    private int intensifyNum = 5;

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
            Invoke("PuzzleSolved", 0.7f);

            PlayerMoneyManager.Instance.AddCoins(10);
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
        if (number > 3)
        {
            Invoke("IntensifyLight", 0.2f);
            return;
        }
        else
        {
            torches[number].GetComponent<HpTorchHandler>().SetFlameLight(true);
            lights[number].GetComponent<Light>().color = Color.gray;
            number++;
            Invoke("PuzzleSolved", 0.5f);
        }
    }

    void FinalColor()
    {
        for (int i = 0; i < 4; i++)
        {
            lights[i].GetComponent<Light>().color = Color.red; 
            lights[i].GetComponent<Light>().intensity = 50;
        }

        FadeLight();
    }

    void FadeLight()
    {
        if(fadeNumber < 13)
        {
            return;
        }
        else
        {
            fadeNumber -= 2;

            for(int i = 0; i < 4; i++)
                lights[i].GetComponent<Light>().intensity = fadeNumber;

            Invoke("FadeLight", 0.1f);
        }
    }

    void IntensifyLight()
    {
        if (intensifyNum > 43)
        {
            FinalColor();
            return;
        }
        else
        {
            intensifyNum += 4;

            for (int i = 0; i < 4; i++)
                lights[i].GetComponent<Light>().intensity = intensifyNum;

            Invoke("IntensifyLight", 0.1f);
        }
    }
}
