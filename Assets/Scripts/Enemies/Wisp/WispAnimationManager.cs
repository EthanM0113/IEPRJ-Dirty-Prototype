using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispAnimationManager : MonoBehaviour
{
    public bool finishedTP = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishedTP()
    {
        finishedTP = true;
    }

    public void NotFinishedTP()
    {
        finishedTP = false;
    }

    public bool GetFinishedTP()
    {
        return finishedTP;
    }

}
