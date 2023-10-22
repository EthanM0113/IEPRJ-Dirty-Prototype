using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTorch : MonoBehaviour
{
    [SerializeField] private GameObject flameLight;
    [SerializeField] private int correctColor;

    private int color = 0;
    private bool isSolved = false;

    public void SetFlameLight(bool isFlameLit)
    {
        if (isFlameLit)
        {
            flameLight.SetActive(true);

            if (color >= 3)
                color = 0;
            else
                color++;

            switch(color)
            {
                case 0:
                    flameLight.GetComponent<Light>().color = Color.red; 
                    break;
                case 1:
                    flameLight.GetComponent<Light>().color = Color.blue;
                    break;
                case 2:
                    flameLight.GetComponent<Light>().color = Color.yellow;
                    break;
                case 3:
                    flameLight.GetComponent<Light>().color = Color.gray;
                    break;
                default:
                    Debug.Log("error in Color Torch");
                    break;
            }
        }
        else
        {
            flameLight.SetActive(false);
        }

        if (color == correctColor)
            isSolved = true;
        else
            isSolved = false;

    }

    public GameObject GetFlameLight()
    {
        return flameLight;
    }

    public bool IsSolved()
    {
        return isSolved;
    }
}
