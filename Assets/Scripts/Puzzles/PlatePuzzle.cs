using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePuzzle : MonoBehaviour
{
    [SerializeField] GameObject platOne, platTwo, platThree;
    [SerializeField] GameObject[] torchGroupOne;
    [SerializeField] GameObject[] torchGroupTwo;
    [SerializeField] GameObject[] torchGroupThree;
    [SerializeField] GameObject[] lightsOne, lightsTwo, lightsThree;

    private int numOne, numTwo, numThree;
    private int prevOne, prevTwo, prevThree;
    private bool solved = false;
    private int timer = 800;
    private bool sentinel = true;
    private bool rewardIsGranted = false;

    void Awake() {
        numOne = platOne.GetComponent<PlateScript>().GetNum();
        numTwo = platTwo.GetComponent<PlateScript>().GetNum();
        numThree = platThree.GetComponent<PlateScript>().GetNum();

        prevOne = numOne;
        prevTwo = numTwo;
        prevThree = numThree;
    }

    void Update() {

        if(!solved) {
            numOne = platOne.GetComponent<PlateScript>().GetNum();
            numTwo = platTwo.GetComponent<PlateScript>().GetNum();
            numThree = platThree.GetComponent<PlateScript>().GetNum();
        }

        if (timer < 800) {
            timer++;
            return;
        }
        else
            sentinel = true;

        if(numOne - prevOne != 0 && sentinel) {
            numTwo++;

            if (numTwo >= 4)
                numTwo = 0;

            platTwo.GetComponent<PlateScript>().SetNum(numTwo);
            timer = 0;
            sentinel = false;
        }
        if (numTwo - prevTwo != 0 && sentinel)
        {
            numOne++;
            numThree++;

            if (numOne >= 4)
                numOne = 0;
            if(numThree >= 4) 
                numThree = 0;
            
            platOne.GetComponent<PlateScript>().SetNum(numOne);
            platThree.GetComponent<PlateScript>().SetNum(numThree);
            timer = 0;
            sentinel = false;
        }
        if (numThree - prevThree != 0 && sentinel)
        {
            numTwo++;

            if (numTwo >= 4)
                numTwo = 0;

            platTwo.GetComponent<PlateScript>().SetNum(numTwo);
            timer = 0;
            sentinel = false;
        }

        if (!solved)
            MatchTorches();

        if (numOne == numTwo && numTwo == numThree && !solved) {
            solved = true;
        }

        prevOne = numOne;
        prevTwo = numTwo;
        prevThree = numThree;

        if(solved && !rewardIsGranted) {
            //PlayerMoneyManager.Instance.AddCoins(10);
            Invoke("SolvedTorches", 2);
            rewardIsGranted = true;
        }
    }

    void MatchTorches()
    {
        for(int i = 0; i < 3; i++)
        {
            torchGroupOne[i].GetComponent<PuzzleIndicator>().SetActivation(false);
            torchGroupTwo[i].GetComponent<PuzzleIndicator>().SetActivation(false);
            torchGroupThree[i].GetComponent<PuzzleIndicator>().SetActivation(false);
        }

        for(int i = 0; i < numOne; i++)
        {
            torchGroupOne[i].GetComponent<PuzzleIndicator>().SetActivation(true);
        }

        for (int i = 0; i < numTwo; i++)
        {
            torchGroupTwo[i].GetComponent<PuzzleIndicator>().SetActivation(true);
        }

        for (int i = 0; i < numThree; i++)
        {
            torchGroupThree[i].GetComponent<PuzzleIndicator>().SetActivation(true);
        }
    }

    void SolvedTorches()
    {
        for (int i = 0; i < numOne; i++)
        {
            lightsOne[i].GetComponent<Light>().color = Color.blue;
        }

        for (int i = 0; i < numTwo; i++)
        {
            lightsTwo[i].GetComponent<Light>().color = Color.blue;
        }

        for (int i = 0; i < numThree; i++)
        {
            lightsThree[i].GetComponent<Light>().color = Color.blue;
        }
    }
}