using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpTorchHandler : MonoBehaviour
{
    [SerializeField] private GameObject flameLight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlameLight(bool isFlameLit)
    {
        if(isFlameLit)
        {
            flameLight.SetActive(true);
        }
        else
        {
            flameLight.SetActive(false);
        }
    }

    public GameObject GetFlameLight()
    {
        return flameLight;
    }
}
