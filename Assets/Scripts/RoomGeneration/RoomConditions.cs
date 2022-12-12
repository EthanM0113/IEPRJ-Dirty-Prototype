using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConditions : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int ODir; //Original Direction
    [SerializeField] private GameObject NDir;
    [SerializeField] private GameObject EDir;
    [SerializeField] private GameObject WDir;
    [SerializeField] private GameObject SDir;
    private bool isFinished = false; //Dictates whether the fighting is finished in the room
    private bool FinGenerated = false; //Dictates whether some of the walls have been removed
    private int Consumes = 1;
    [SerializeField] private int RoomID;

    [SerializeField] GameObject[] extraNodes;
    [SerializeField] GameObject[] extraObjects;
    [SerializeField] GameObject plane;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int totalDead = 0;
    bool isStart = false;



    private void generateMisc(GameObject Misc, GameObject obj)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < Misc.transform.childCount; i++)
        {
            list.Add(Misc.transform.GetChild(i));
        }
        for (int i = 0; i < list.Count; i++)
        {
           
                Instantiate(obj, list[i]);
                Debug.Log("Created Item");
            
        }
    }
    public void enemySetup()
    {
        totalEnemies = FindObjectOfType<LevelRouteHandler>().GetRouteList();
    }
    public void enemyKill()
    {
        totalDead++;
    }
   
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


    private void Awake()
    {
        for (int i = 0; i < extraNodes.Length; i++)
        {
            generateMisc(extraNodes[i], extraObjects[i]);
        }
        enemySetup();
        isStart = true;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (totalEnemies == totalDead)
        {
            DeadEnemies();
        }
        if (isFinished && !FinGenerated)
        {
            detectWalls();
        }
    }

    public void detectWalls()
    {
        int num = Random.Range(1, 4);
        List<GameObject> RoomList =  new List<GameObject>();
        RoomList.Add(NDir);
        RoomList.Add(EDir);
        RoomList.Add(WDir);
        RoomList.Add(SDir);
        switch (ODir)
        {
            case 0://Origin is Bottom
                RoomList.RemoveAt(3);
                break;
            case 1: //Origin is Right
                RoomList.RemoveAt(2);
                break;
            case 2://Origin is Left
                RoomList.RemoveAt(1);
                break;
            case 3://Origin is Top
                RoomList.RemoveAt(0);
                    break;
        }
        for (int i = 0; i < num; i++)
        {
            int gen = Random.Range(0, RoomList.Count);
            RoomList[gen].transform.GetChild(0).gameObject.SetActive(false);
            RoomList.RemoveAt(gen);
            Debug.Log("Removed Wall");
        }
        FinGenerated = true;
    }

    public int GetRoomID()
    {
        return RoomID;
    }
    
    public int GetConsumes()
    {
        return Consumes;
    }

    public void RemoveConsumes()
    {
        Consumes--;
    }

    public void DeadEnemies()
    {
        isFinished = true;
    }

    


}
