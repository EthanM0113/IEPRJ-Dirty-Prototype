using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : MonoBehaviour
{
    [SerializeField] private GameObject[] cones;
    int timer = 50;
    int curCone = 0;

    void Awake()
    {
        foreach (var c in cones)
            c.gameObject.SetActive(false);
    }

    void Update()
    {
        timer--;

        if (timer == 0)
            ActivateSpike();
    }

    void ActivateSpike()
    {
        cones[curCone].gameObject.SetActive(true);

        if (curCone > 0)
            cones[curCone - 1].gameObject.SetActive(false);
        else
            cones[6].gameObject.SetActive(false);

        curCone++;

        if (curCone > 6)
            curCone = 0;

        timer = 50;
    }
}
