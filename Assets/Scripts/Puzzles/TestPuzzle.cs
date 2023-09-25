using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzle : MonoBehaviour
{
    [SerializeField] GameObject platOne, platTwo, platThree;

    private int numOne, numTwo, numThree;
    private int prevOne, prevTwo, prevThree;
    private bool solved = false;
    private int timer = 800;
    private bool sentinel = true;

    void Awake() {
        numOne = platOne.GetComponent<TestPlatform>().GetNum();
        numTwo = platTwo.GetComponent<TestPlatform>().GetNum();
        numThree = platThree.GetComponent<TestPlatform>().GetNum();

        prevOne = numOne;
        prevTwo = numTwo;
        prevThree = numThree;
    }

    void Update() {

        if(!solved) {
            numOne = platOne.GetComponent<TestPlatform>().GetNum();
            numTwo = platTwo.GetComponent<TestPlatform>().GetNum();
            numThree = platThree.GetComponent<TestPlatform>().GetNum();
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

            platTwo.GetComponent<TestPlatform>().SetNum(numTwo);
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
            
            platOne.GetComponent<TestPlatform>().SetNum(numOne);
            platThree.GetComponent<TestPlatform>().SetNum(numThree);
            timer = 0;
            sentinel = false;
        }
        if (numThree - prevThree != 0 && sentinel)
        {
            numTwo++;

            if (numTwo >= 4)
                numTwo = 0;

            platTwo.GetComponent<TestPlatform>().SetNum(numTwo);
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
    }
}
