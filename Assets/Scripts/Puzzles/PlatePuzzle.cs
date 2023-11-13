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

        if(numOne - prevOne != 0) {
            numTwo++;

            if (numTwo >= 4)
                numTwo = 0;

            platTwo.GetComponent<PlateScript>().SetNum(numTwo);
        }
        else if (numTwo - prevTwo != 0)
        {
            numOne++;
            numThree++;

            if (numOne >= 4)
                numOne = 0;
            if(numThree >= 4) 
                numThree = 0;
            
            platOne.GetComponent<PlateScript>().SetNum(numOne);
            platThree.GetComponent<PlateScript>().SetNum(numThree);
        }
        else if (numThree - prevThree != 0)
        {
            numTwo++;

            if (numTwo >= 4)
                numTwo = 0;

            platTwo.GetComponent<PlateScript>().SetNum(numTwo);
        }

        if (!solved)
            MatchTorches();

        if (numOne == 3 && numTwo == 3 && numThree == 3 && !solved) {
            solved = true;
            Invoke("SolvedTorches", 2);
        }

        prevOne = numOne;
        prevTwo = numTwo;
        prevThree = numThree;

        if(solved && !rewardIsGranted) {
            SoundManager.Instance.BackstabHit();
            PlayerMoneyUIHandler playerMoneyUIHandler = FindObjectOfType<PlayerMoneyUIHandler>();
            playerMoneyUIHandler.SpinCoinImage();
            playerMoneyUIHandler.PulseCointText();
            PlayerMoneyManager.Instance.AddCoins(20); // enough for one upgrade to incentivize completing the puzzle         
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