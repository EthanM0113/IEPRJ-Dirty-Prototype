using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePuzzle : MonoBehaviour
{
    [SerializeField] GameObject platOne, platTwo, platThree;

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
                numTwo = 0;
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

        if (numOne == numTwo && numTwo == numThree && !solved) {
            solved = true;
        }

        prevOne = numOne;
        prevTwo = numTwo;
        prevThree = numThree;

        Debug.Log(numOne + " - " + numTwo + " - " + numThree + " - " + solved);

        if(solved && !rewardIsGranted) {
            PlayerMoneyManager.Instance.AddCoins(10);
            rewardIsGranted = true;
        }
    }
}
