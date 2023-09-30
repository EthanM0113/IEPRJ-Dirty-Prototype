using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    [SerializeField] private GameObject puzzleBrain;

    public bool state;
    public int torchNum;

    void OnTriggerEnter(Collider other)
    {
        if (!puzzleBrain.GetComponent<TorchPuzzle>().GetIsSolved())
        {
            if (other.gameObject.tag == "Player")
            {
                state = !state;
                puzzleBrain.GetComponent<TorchPuzzle>().TorchActivated(torchNum);
            }
        }
    }

    public bool GetState() {
        return state;
    }
}
