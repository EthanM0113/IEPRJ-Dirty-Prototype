using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] Templates;
    public GameObject BaseRoom;
    public GameObject ClosedRoom;
    [SerializeField] protected int MaxRooms = 0;
    
    protected int DeadEndCD = 3;


    public int GetDeadEnds()
    {
        return DeadEndCD;
    }

    public void ResetCD()
    {
        DeadEndCD = 3;
    }

    public void DecrementCD()
    {
        DeadEndCD--;
    }

    public void DecrementMaxRooms()
    {
        MaxRooms--;
    }
    public int GetMaxRooms()
    {
        return MaxRooms;
    }

}
