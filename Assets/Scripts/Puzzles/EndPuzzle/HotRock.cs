using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotRock : MonoBehaviour
{
    [SerializeField] private GameObject hotRock;
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject westTorch;

    private bool solved = false;
    private int correctColor = 0;

    void Awake()
    {
        correctColor = westTorch.GetComponent<ColorTorch>().GetCorrectColor();
    }

    public void Interact()
    {
        if (solved)
            return;
        
        if (playerInventory.GetComponent<PlayerInventory>().GetIceChunks())
        {
            hotRock.GetComponent<LoreObject>().UpdateText("Right, how about ice chunks?");
            Invoke("IceChunkApplied", 1.5f);

            playerInventory.GetComponent<PlayerInventory>().SetIceChunks(false);
            solved = true;

            Invoke("CooledText", 3f);

            switch (correctColor)
            {
                case 0:
                    hotRock.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 1:
                    hotRock.GetComponent<Renderer>().material.color = Color.blue;
                    break;
                case 2:
                    hotRock.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                case 3:
                    hotRock.GetComponent<Renderer>().material.color = Color.gray;
                    break;
            }
        }
        else if (playerInventory.GetComponent<PlayerInventory>().GetWater())
        {
            hotRock.GetComponent<LoreObject>().UpdateText("The water sizzles and evaporates. . .");
            playerInventory.GetComponent<PlayerInventory>().SetWater(false);
        }
    }

    private void CooledText()
    {
        hotRock.GetComponent<LoreObject>().UpdateText("This color... could it have something to do with resonance?");
    }

    private void IceChunkApplied()
    {
        hotRock.GetComponent<LoreObject>().UpdateText("It cooled down!");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!solved)
                hotRock.GetComponent<LoreObject>().SetText("The intense heat obscures its true color, perhaps it can be cooled down.");
        }
    }
}
