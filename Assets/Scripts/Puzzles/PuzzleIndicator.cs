using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleIndicator : MonoBehaviour
{
    [SerializeField] GameObject lightSource;
    bool isActivated = false;

    void Start()
    {
        lightSource.SetActive(false);
        isActivated = false;
    }

    void Update()
    {
        if (isActivated)
            lightSource.SetActive(true);
        else
            lightSource.SetActive(false);
    }

    public void SetActivation(bool value)
    {
        isActivated = value;
    }
}