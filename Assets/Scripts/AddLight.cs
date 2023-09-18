using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLight : MonoBehaviour
{
    private LightContainer container;
    private void Awake()
    {
       container = GameObject.FindGameObjectWithTag("LightContainer").GetComponent<LightContainer>();
       container.LightList.Add(this.gameObject.GetComponent<Light>());
    }
}
