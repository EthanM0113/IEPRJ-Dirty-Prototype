using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetRock : MonoBehaviour
{
    [SerializeField] private GameObject wetRock;
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject eastTorch;

    private bool solved = false;
    private int correctColor = 0;

    void Awake()
    {
        correctColor = eastTorch.GetComponent<ColorTorch>().GetCorrectColor();
    }

    public void Interact()
    {
        if (solved)
            return;

        if (playerInventory.GetComponent<PlayerInventory>().GetScroll())
        {
            wetRock.GetComponent<LoreObject>().UpdateText("Ah, I have a freezing scroll. . . There, it's frozen solid!");
            playerInventory.GetComponent<PlayerInventory>().SetIceChunks(false);
            solved = true;

            Invoke("FrozenText", 2.5f);

            switch(correctColor)
            {
                case 0:
                    wetRock.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 1:
                    wetRock.GetComponent<Renderer>().material.color = Color.blue;
                    break;
                case 2:
                    wetRock.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                case 3:
                    wetRock.GetComponent<Renderer>().material.color = Color.gray;
                    break;
            }
        }
        else if (playerInventory.GetComponent<PlayerInventory>().GetWater())
        {
            wetRock.GetComponent<LoreObject>().UpdateText("Well, now it's extra wet.");
            playerInventory.GetComponent<PlayerInventory>().SetWater(false);
        }
    }

    private void FrozenText()
    {
        wetRock.GetComponent<LoreObject>().UpdateText("This color... could it have something to do with resonance?");
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!solved)
                wetRock.GetComponent<LoreObject>().SetText("Water obscures its true color, perhaps it can be frozen?");
        }
    }
}
