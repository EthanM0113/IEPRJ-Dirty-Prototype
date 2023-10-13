using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    [SerializeField] GameObject torchOne, torchTwo, torchThree, torchFour, torchFive, torchSix;
    [SerializeField] GameObject[] lights;

    [SerializeField] bool one, two, three, four, five, six;
    private bool solved = false;
    private bool rewardIsGranted = false;
    private int number = 0;


    void Awake()
    {
        torchOne.GetComponent<TorchScript>().SetState(one);
        torchTwo.GetComponent<TorchScript>().SetState(two);
        torchThree.GetComponent<TorchScript>().SetState(three);
        torchFour.GetComponent<TorchScript>().SetState(four);
        torchFive.GetComponent<TorchScript>().SetState(five);
        torchSix.GetComponent<TorchScript>().SetState(six);
    }

    void Update()
    {
        if(one && two && three && four && five && six && !solved) {
            solved = true;
            Invoke("SolvedTorches", 2f);
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
                torchSix.GetComponent<TorchScript>().SetState(six);
                torchTwo.GetComponent<TorchScript>().SetState(two);
                break;
            case 2:
                one = !one;
                two = !two;
                three = !three;
                torchOne.GetComponent<TorchScript>().SetState(one);
                torchThree.GetComponent<TorchScript>().SetState(three);
                break;
            case 3:
                two = !two;
                three = !three;
                four = !four;
                torchTwo.GetComponent<TorchScript>().SetState(two);
                torchFour.GetComponent<TorchScript>().SetState(four);
                break;
            case 4:
                three = !three;
                four = !four;
                five = !five;
                torchThree.GetComponent<TorchScript>().SetState(three);
                torchFive.GetComponent<TorchScript>().SetState(five);
                break;
            case 5:
                four = !four;
                five = !five;
                six = !six;
                torchFour.GetComponent<TorchScript>().SetState(four);
                torchSix.GetComponent<TorchScript>().SetState(six);
                break;
            case 6:
                five = !five;
                six = !six;
                one = !one;
                torchFive.GetComponent<TorchScript>().SetState(five);
                torchOne.GetComponent<TorchScript>().SetState(one);
                break;
            default:
                break;
        }
    }

    public bool GetIsSolved()
    {
        return solved;
    }

    void SolvedTorches()
    {
        if (number > 5)
        {
            Invoke("FinalColor", 0.5f);
            return;
        }
        else
        {
            lights[number].GetComponent<Light>().color = Color.yellow;
            number++;
            Invoke("SolvedTorches", 0.1f);
        }
    }

    void FinalColor()
    {
        for(int i = 0; i < 6; i++)
        {
            lights[i].GetComponent<Light>().color = Color.blue;
        }
    }
}
