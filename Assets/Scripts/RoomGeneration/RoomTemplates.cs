using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] SneakTemplates; //Enemy Rooms
    public GameObject[] PuzzleTemplates; //Puzzle Rooms
    public GameObject[] ExtraTemplates; //Lore and Rest Rooms
    public GameObject BaseRoom;
    public GameObject ClosedRoom;
    protected int MaxRooms;
    [SerializeField] protected int TotalRooms; //Does not decrement
    [SerializeField] public List<GameObject> interiors;
    protected int DeadEndCD = 3;

    public float waitTime;
    private bool spawnedBoss = false;
    private bool spawnedPuzzle = false;
    private bool spawnedExtra = false;
    private bool isLastSpawnedExtra = false;

    public GameObject EndRoomTemplate;

    void Start()
    {
        MaxRooms = TotalRooms;
    }
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
    public int GetTotalRooms()
    {
        return TotalRooms;
    }

    public bool GetPity(int type)
    {
        if (type == 1)
        {
            return spawnedPuzzle;
        }
        return spawnedExtra;
    }

    public void TogglePity(int type, bool isActive)
    {
        if (type == 1)
        {
            spawnedPuzzle = isActive;
        }
        else if (type == 2)
        {
            spawnedExtra = isActive;
        }
        else
        {
            isLastSpawnedExtra = isActive;
        }
    }

    public bool GetLastSpawned()
    {
        return isLastSpawnedExtra;
    }

    private void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < interiors.Count; i++)
            {
                if (i == interiors.Count - 1)
                {
                    GameObject BossRoom = Instantiate(EndRoomTemplate, interiors[i].transform.position, Quaternion.identity, interiors[i].transform.parent);
                    Destroy(interiors[i]);
                    interiors.RemoveAt(i);
                    interiors.Add(BossRoom);
                    spawnedBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
