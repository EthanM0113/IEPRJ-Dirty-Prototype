using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RoomProperties : MonoBehaviour
{
    [SerializeField] private GameObject TD; //TD = Top Door of the Room
    [SerializeField] private GameObject LD;
    [SerializeField] private GameObject RD;
    [SerializeField] private GameObject BD;
    [SerializeField] public bool IsStart = false;
    private int rand;

    private void Awake()
    {
        if (IsStart)
        {
            TD.GetComponent<ToggleOpening>().OpenWall();
            LD.GetComponent<ToggleOpening>().OpenWall();
            RD.GetComponent<ToggleOpening>().OpenWall();
            BD.GetComponent<ToggleOpening>().OpenWall();
        }
    }
    public void RoomSetup(int dir, bool isClosed)
    {
        List<GameObject> Roomlist;
        List<GameObject> RoomRand;
        Roomlist = new List<GameObject>();
        RoomRand = new List<GameObject>();
        switch (dir)
        {
            //Remove the wall based on the opening direction
            case 1: //Needs a bottom door
                Roomlist.Add(BD);
                RoomRand.Add(RD);
                RoomRand.Add(LD); 
                RoomRand.Add(TD);

                break;
            case 2:
  
                Roomlist.Add(TD);
                RoomRand.Add(RD);
                RoomRand.Add(LD);
                RoomRand.Add(BD);
                break;
            case 3:

                Roomlist.Add(LD);
                RoomRand.Add(RD);
                RoomRand.Add(TD);
                RoomRand.Add(BD);
                break;
            case 4:

                Roomlist.Add(RD);
                RoomRand.Add(LD);
                RoomRand.Add(TD);
                RoomRand.Add(BD);

                break;
        }
        if (!isClosed) //More than one neighboor
        {
           rand = Random.Range(0, RoomRand.Count);
           Roomlist.Add(RoomRand[rand]);
           RoomRand.RemoveAt(rand);
           for(int i = 0; i < RoomRand.Count; i++) {
                rand = Random.Range(0, 101);
                if (rand > 49)
                {
                    Roomlist.Add(RoomRand[i]);
                }
            }
        }
       
        //Determine Room Shape
        for (int i = 0; i < Roomlist.Count; i++)
        {
            Roomlist[i].GetComponent<ToggleOpening>().OpenWall();
        }
    }
}
