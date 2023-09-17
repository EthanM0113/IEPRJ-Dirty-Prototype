using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOpening : MonoBehaviour
{
    [SerializeField] GameObject Wall;
    [SerializeField] GameObject SpawnNode;
    
    public void CloseWall()
    {
        Wall.SetActive(true);
        SpawnNode.SetActive(false);
    }
    public void OpenWallV1()
    {
        Wall.SetActive(false);
        SpawnNode.SetActive(true);
    }
    public void OpenWallV2()
    {
        Wall.SetActive(false);
        SpawnNode.SetActive(true);
        SpawnNode.GetComponent<RoomSpawner>().StartRoomGen();
    }


}
