using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    [SerializeField] private GameObject puzzleBrain;

    private bool state = false;
    [SerializeField] private int torchNum;

    [SerializeField] GameObject lightSource;
    bool isActivated = false;

    void Update()
    {
        if (isActivated)
            lightSource.SetActive(true);
        else
            lightSource.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!puzzleBrain.GetComponent<TorchPuzzle>().GetIsSolved())
        {
            if (other.gameObject.tag == "Player")
            {
                state = !state;
                isActivated = state;
                puzzleBrain.GetComponent<TorchPuzzle>().TorchActivated(torchNum);
            }
        }
    }

    public bool GetState() {
        return state;
    }

    public void SetState(bool newState)
    {
        state = newState;
        isActivated = state;
    }
}
