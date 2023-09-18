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
    [SerializeField] public List<GameObject> interiors;
    protected int DeadEndCD = 3;

    public float waitTime;
    private bool spawnedBoss = false;
    public GameObject BossTemplate;

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

    private void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < interiors.Count; i++)
            {
                if (i == interiors.Count - 1)
                {
                    Instantiate(BossTemplate, interiors[i].transform.position, Quaternion.identity, interiors[i].transform.parent);
                    Destroy(interiors[i]);
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
