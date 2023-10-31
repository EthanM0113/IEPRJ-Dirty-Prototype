using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBOrbManager : MonoBehaviour
{
    private float ticks = 0f;
    private float lifetime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ticks += Time.deltaTime;
        if(ticks > lifetime) 
        { 
            Destroy(gameObject);
        }
    }
}
