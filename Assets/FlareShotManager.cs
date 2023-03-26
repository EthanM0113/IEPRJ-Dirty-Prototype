using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotManager : MonoBehaviour
{
    [SerializeField] private float LIFE_DURATION = 2.0f;
    private float ticks = 0.0f;
  
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        ticks += Time.deltaTime;
        if(ticks >= LIFE_DURATION)
        {
            gameObject.SetActive(false);
            ticks = 0.0f;
        }
    }
}
