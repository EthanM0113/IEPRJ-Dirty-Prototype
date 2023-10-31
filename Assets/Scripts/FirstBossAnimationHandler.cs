using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnimationHandler : MonoBehaviour
{
    public bool isAttacking;
    

    // Start is called before the first frame update
    void Start()
    {
        // For debugging
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishedAttacking()
    {
        isAttacking = false;
    }

    public void IsAttacking()
    {
        isAttacking = true;
    }

}
