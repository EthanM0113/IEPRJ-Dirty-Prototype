using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticStone : MonoBehaviour
{
    [SerializeField] private GameObject stone;

    private bool isHit = false;

    void Awake()
    {
        stone.GetComponent<Renderer>().material.color = Color.gray;
    }

    public void ChangeColor()
    {
        stone.GetComponent<Renderer>().material.color = Color.yellow;
        isHit = true;

        stone.GetComponent<LoreObject>().UpdateText("Smacking it seems to make it change color?");
        Invoke("RevertColor", 2f);
    }

    private void RevertColor()
    {
        stone.GetComponent<Renderer>().material.color = Color.gray;
    }
}
