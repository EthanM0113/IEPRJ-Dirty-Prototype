using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRender : MonoBehaviour
{
    [SerializeField] private Light[] limelight;

    private void OnPreCull()
    {
        
        //limelight.enabled = false;
    }
    private void OnPreRender()
    {
       // limelight.enabled = false;
    }
    private void OnPostRender()
    {
       // limelight.enabled = true;
    }
}
