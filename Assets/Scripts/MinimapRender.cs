using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRender : MonoBehaviour
{
    private LightContainer container;

    private void OnPreCull()
    {
        for (int i = 0; i < container.LightList.Count; i++)
        {
            container.LightList[i].enabled = false;
        }
        
        //limelight.enabled = false;
    }
    private void OnPreRender()
    {
        for (int i = 0; i < container.LightList.Count; i++)
        {
            container.LightList[i].enabled = false;
        }
    }
    private void OnPostRender()
    {
        for (int i = 0; i < container.LightList.Count; i++)
        {
            container.LightList[i].enabled = true;
        }
    }
}
