using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MiniTorchPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] torches;
    [SerializeField] private bool one, two, three;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject northTorch;

    private bool isSolved, flashed;
    private int number = 0;
    private int fadeNumber = 50;
    private int intensifyNum = 5;
    private int correctColor = 0;

    void Awake()
    {
        isSolved = false;
        flashed = false;
        torches[0].GetComponent<MiniTorch>().SetState(one);
        torches[1].GetComponent<MiniTorch>().SetState(two);
        torches[2].GetComponent<MiniTorch>().SetState(three);

        correctColor = northTorch.GetComponent<ColorTorch>().GetCorrectColor();
    }

    void Update()
    {
        if(one && two && three)
            isSolved = true;

        if(isSolved && !flashed)
        {
            Invoke("PuzzleSolved", 0.7f);
            flashed = true;
        }
    }

    public void TorchActivated(int torchNum)
    {
        switch (torchNum)
        {
            case 1:
                one = !one;
                two = !two;
                torches[0].GetComponent<MiniTorch>().SetState(one);
                torches[1].GetComponent<MiniTorch>().SetState(two);
                break;
            case 2:
                two = !two;
                torches[1].GetComponent<MiniTorch>().SetState(two);
                break;
            case 3:
                three = !three;
                two = !two;
                torches[2].GetComponent<MiniTorch>().SetState(three);
                torches[1].GetComponent<MiniTorch>().SetState(two);
                break;
        }
    }

    public bool GetIsSolved()
    {
        return isSolved;
    }

    void PuzzleSolved()
    {
        if (number > 2)
        {
            Invoke("IntensifyLight", 0.2f);
            return;
        }
        else
        {
            torches[number].GetComponent<MiniTorch>().SetState(true);
            lights[number].GetComponent<Light>().color = Color.gray;
            number++;
            Invoke("PuzzleSolved", 0.5f);
        }
    }

    void FinalColor()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (correctColor)
            {
                case 0:
                    lights[i].GetComponent<Light>().color = Color.red;
                    break;
                case 1:
                    lights[i].GetComponent<Light>().color = Color.blue;
                    break;
                case 2:
                    lights[i].GetComponent<Light>().color = Color.yellow;
                    break;
                case 3:
                    lights[i].GetComponent<Light>().color = Color.gray;
                    break;
            }
            
            lights[i].GetComponent<Light>().intensity = 50;
        }

        FadeLight();
    }

    void FadeLight()
    {
        if (fadeNumber < 13)
        {
            return;
        }
        else
        {
            fadeNumber -= 2;

            for (int i = 0; i < 3; i++)
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

            for (int i = 0; i < 3; i++)
                lights[i].GetComponent<Light>().intensity = intensifyNum;

            Invoke("IntensifyLight", 0.1f);
        }
    }
}