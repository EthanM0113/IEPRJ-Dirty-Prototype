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
                NDir.SetActive(true);
                EDir.SetActive(true);
                WDir.SetActive(true);
                SDir.SetActive(false);
                break;
            case 1:
                ODir = 1;
                NDir.SetActive(true);
                EDir.SetActive(true);
                WDir.SetActive(false);
                SDir.SetActive(true);
                break;
            case 2:
                ODir = 2;
                NDir.SetActive(true);
                EDir.SetActive(false);
                WDir.SetActive(true);
                SDir.SetActive(true);
                break;
            case 3:
                ODir = 3;
                NDir.SetActive(false);
                EDir.SetActive(true);
                WDir.SetActive(true);
                SDir.SetActive(true);
                break;

        }
        RoomID = ID;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
