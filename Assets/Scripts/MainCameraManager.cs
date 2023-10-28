using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject blackBar1;
    [SerializeField] private GameObject blackBar2;
    [SerializeField] private GameObject gargoyleFX;
    [SerializeField] private Animator animator;
    private bool pulseTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        ToggleBlackBars(false);
        ToggleGargoyleFX(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleBlackBars(bool flag)
    {
        if(flag) 
        {
            blackBar1.SetActive(true);
            blackBar2.SetActive(true);
        }
        else 
        {
            blackBar1.SetActive(false);
            blackBar2.SetActive(false);
        }
    }

    public void ToggleGargoyleFX(bool flag)
    {
        gargoyleFX.SetActive(flag);
        if(flag == true && !pulseTriggered)
        {
            animator.SetTrigger("isGargoylePulsing");
            pulseTriggered = true;
        }
        else if(flag == false && pulseTriggered)
        {
            pulseTriggered = false;
        }
    }
}
