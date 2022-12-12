using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossRoom : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int ODir; //Original Direction
    [SerializeField] private GameObject NDir;
    [SerializeField] private GameObject EDir;
    [SerializeField] private GameObject WDir;
    [SerializeField] private GameObject SDir;
    [SerializeField] private int RoomID;
    public void setup(int dir, int ID)
    {
        Debug.Log("Setting up Room...");
        switch (dir)
        {
            case 0:
                ODir = 0;
                SDir.SetActive(false);
                break;
            case 1:
                ODir = 1;
                WDir.SetActive(false);
                break;
            case 2:
                ODir = 2;
                EDir.SetActive(false);
                break;
            case 3:
                ODir = 3;
                NDir.SetActive(false);
                break;

        }
        RoomID = ID;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
