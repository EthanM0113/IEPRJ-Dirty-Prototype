using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    [SerializeField] GameObject torchOne, torchTwo, torchThree, torchFour, torchFive, torchSix;
    private bool one = false, two = false, three = false, four = false, five = false, six = false;
    private bool solved = false;
    private bool rewardIsGranted = false;

    void Awake()
    {
        one = torchOne.GetComponent<TorchScript>().GetState();
        two = torchTwo.GetComponent<TorchScript>().GetState();
        three = torchThree.GetComponent<TorchScript>().GetState();
        four = torchFour.GetComponent<TorchScript>().GetState();
        five = torchFive.GetComponent<TorchScript>().GetState();
        six = torchSix.GetComponent<TorchScript>().GetState();

    }

    void Update()
    {
        if(one && two && three && four && five && six) {
            solved = true;
        }

        if (solved && !rewardIsGranted)
        {
            PlayerMoneyManager.Instance.AddCoins(10);
            rewardIsGranted = true;
        }
    }

    // 1 2 3 4 5 6
    public void TorchActivated(int torchNum)
    {
        switch(torchNum)
        {
            case 1:
                six = !six;
                one = !one;
                two = !two;
                break;
            case 2:
                one = !one;
                two = !two;
                three = !three;
                break;
            case 3:
                two = !two;
                three = !three;
                four = !four;
                break;
            case 4:
                three = !three;
                four = !four;
                five = !five;
                break;
            case 5:
                four = !four;
                five = !five;
                six = !six;
                break;
            case 6:
                five = !five;
                six = !six;
                one = !one;
                break;
            default:
                break;
        }
    }

    public bool GetIsSolved()
    {
        return solved;
    }
}
