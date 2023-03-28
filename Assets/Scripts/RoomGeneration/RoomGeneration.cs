using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField] GameObject RespawnNode;
    private bool create = false;
    [SerializeField] GameObject[] PrefabList;
    [SerializeField] GameObject BossPrefab;
    [SerializeField] private int roomsGenerated = 1;
    [SerializeField] private int bossRoomNo = 2;
    [SerializeField] private bool isTutorial = false;
     
    private void OnCollisionStay(Collision collision)
    {
        RoomConditions room;
       if ( room = collision.gameObject.GetComponentInParent<RoomConditions>())
        Debug.Log($"Room ID {room.GetRoomID()}");



    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject wall = other.transform.parent.GameObject().transform.parent.gameObject;
        if (other.gameObject.tag == "NorthWall" )
        {
            CreateRoom(0, wall);
            Debug.Log("Coordinates: " + wall.transform.position.x + " " +
                      wall.transform.position.y + " " +
                wall.transform.position.z);
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        else if (other.gameObject.tag == "EastWall")
        {
            CreateRoom(1,wall);
            Debug.Log("Coordinates: " + wall.transform.position.x + " " +
                      wall.transform.position.y + " " +
                      wall.transform.position.z);
            
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;
        }
        else if (other.gameObject.tag == "WestWall")
        {
            CreateRoom(2, wall);
            Debug.Log("Coordinates: " + wall.transform.position.x + " " +
                      wall.transform.position.y + " " +
                      wall.transform.position.z);
            
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        else if (other.gameObject.tag == "SouthWall")
        {
            CreateRoom(3, wall);
            Debug.Log("Coordinates: " + wall.transform.position.x + " " +
                      wall.transform.position.y + " " +
                      wall.transform.position.z);
            
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        

    }
    private void CreateRoom (int direction, GameObject collisionRef)
    {
        Debug.Log("CREATE ROOM");
        int chosen = Random.Range(0, PrefabList.Length);
        GameObject room;
        Vector3 place = collisionRef.transform.position;

        if (roomsGenerated == bossRoomNo)//Set this for boss
        {
            switch (direction)
            {
                case 0:
                    room = Instantiate(BossPrefab, new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;

                    break;
                case 1:
                    room = Instantiate(BossPrefab, new Vector3(place.x + 20, place.y, place.z), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 2:
                    room = Instantiate(BossPrefab, new Vector3(place.x - 20, place.y, place.z), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 3:
                    room = Instantiate(BossPrefab, new Vector3(place.x, place.y, place.z - 20), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
            }
        }
        else if (!isTutorial) //Normal Level Room Generation
        {
            switch (direction)
            {
                case 0:
                    room = Instantiate(PrefabList[chosen], new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;


                    break;
                case 1:
                    room = Instantiate(PrefabList[chosen], new Vector3(place.x + 20, place.y, place.z), Quaternion.identity) as GameObject;
                    room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 2:
                    room = Instantiate(PrefabList[chosen], new Vector3(place.x - 20, place.y, place.z), Quaternion.identity) as GameObject;
                    room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 3:
                    room = Instantiate(PrefabList[chosen], new Vector3(place.x, place.y, place.z - 20), Quaternion.identity) as GameObject;
                    room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
            }
        }
        else //This is when Scene is a Tutorial Level
        {
            switch (roomsGenerated)
            {
                case 1:
                    room = Instantiate(PrefabList[0], new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;
                case 2:
                    room = Instantiate(PrefabList[1], new Vector3(place.x, place.y, place.z + 30), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;
                case 3:
                    room = Instantiate(PrefabList[2], new Vector3(place.x, place.y, place.z + 30), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;
                case 4:
                    room = Instantiate(PrefabList[3], new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;
                case 5:
                    room = Instantiate(PrefabList[4], new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;
                case 6:
                    room = Instantiate(PrefabList[5], new Vector3(place.x, place.y, place.z + 20), Quaternion.identity) as GameObject;
                    roomsGenerated++;
                    break;

            }
        }

    }
    public int GetRoomsGenerated()
    {
        return roomsGenerated;
    }

    public int GetBossRoomNo()
    {
        return bossRoomNo;
    }
   
}
