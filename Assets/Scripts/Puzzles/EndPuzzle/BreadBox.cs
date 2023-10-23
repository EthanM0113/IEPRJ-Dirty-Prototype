using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBox : MonoBehaviour
{
    [SerializeField] private GameObject playerInv;

    private int breadNum;

    void Awake()
    {
        breadNum = 0;
    }

    void Update()
    {
        if (breadNum >= 4)
        {
            // TODO: put text of get key
            playerInv.GetComponent<PlayerInventory>().SetKey(true);
        }
        else
        {
            // TODO: put text of get bread
            TakeBread();
            playerInv.GetComponent<PlayerInventory>().SetBread(breadNum);
        }
    }

    void TakeBread()
    {
        breadNum++;
    }
}
