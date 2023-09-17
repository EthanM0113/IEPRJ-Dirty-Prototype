using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    [SerializeField] private GameObject TD; //TD = Top Door of the Room
    [SerializeField] private GameObject LD;
    [SerializeField] private GameObject RD;
    [SerializeField] private GameObject BD;
    public void RoomSetup(int dir)
    {
        switch(dir)
        {
            //Remove the wall based on the opening direction
            case 1: //Needs a bottom door
                BD.SetActive(false);
                break;
            case 2:
                RD.SetActive(false);
                break;
            case 3:
                LD.SetActive(false);
                break;
            case 4:
                TD.SetActive(false);
                break;
        }
    }
}
