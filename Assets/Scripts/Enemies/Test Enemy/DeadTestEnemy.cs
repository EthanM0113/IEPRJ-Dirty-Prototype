using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTestEnemy : BaseDeadEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Tick();
    }
}
