using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTorch : MonoBehaviour
{
    [SerializeField] private GameObject puzzleBrain;

    private bool state;
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
        if (!puzzleBrain.GetComponent<MiniTorchPuzzle>().GetIsSolved())
        {
            if (other.gameObject.tag == "Player")
            {
                state = !state;
                isActivated = state;
                puzzleBrain.GetComponent<MiniTorchPuzzle>().TorchActivated(torchNum);
            }
        }
    }

    public void SetState(bool newState)
    {
        state = newState;
        isActivated = state;
    }
}
