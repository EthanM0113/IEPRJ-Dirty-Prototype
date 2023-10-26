using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticStone : MonoBehaviour
{
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject southTorch;
    private int correctColor = 0;

    void Awake()
    {
        stone.GetComponent<Renderer>().material.color = Color.gray;
        correctColor = southTorch.GetComponent<ColorTorch>().GetCorrectColor();
    }

    public void ChangeColor()
    {
        switch (correctColor)
        {
            case 0:
                stone.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 1:
                stone.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 2:
                stone.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                stone.GetComponent<Renderer>().material.color = Color.gray;
                break;
        }

        stone.GetComponent<LoreObject>().UpdateText("Smacking it seems to make it change color?");
        Invoke("RevertColor", 2f);
    }

    private void RevertColor()
    {
        stone.GetComponent<Renderer>().material.color = Color.gray;
    }
}
