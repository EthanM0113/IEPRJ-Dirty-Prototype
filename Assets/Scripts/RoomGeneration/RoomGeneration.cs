using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField] GameObject RespawnNode;
    private bool create = false;
    [SerializeField] GameObject[] PrefabList;
    [SerializeField] GameObject BossPrefab;
    [SerializeField] private int roomsGenerated = 1;
    //[SerializeField] GameObject footCollider;
     
    private void OnCollisionStay(Collision collision)
    {
        RoomConditions room;
       if ( room = collision.gameObject.GetComponentInParent<RoomConditions>())
        Debug.Log($"Room ID {room.GetRoomID()}");



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NorthWall" )
        {
            CreateRoom(0, other.gameObject);
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        else if (other.gameObject.tag == "EastWall")
        {
            CreateRoom(1, other.gameObject);
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;
        }
        else if (other.gameObject.tag == "WestWall")
        {
            CreateRoom(2, other.gameObject);
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        else if (other.gameObject.tag == "SouthWall")
        {
            CreateRoom(3, other.gameObject);
            other.gameObject.SetActive(false);
            RespawnNode.transform.position = this.transform.position;

        }
        

    }
    private void CreateRoom (int direction, GameObject collisionRef)
    {
        Debug.Log("CREATE ROOM");
        int chosen = Random.Range(0, PrefabList.Length);
        //int chosen = 0;
        GameObject room;
        Vector3 place = collisionRef.transform.position;
        
        if (roomsGenerated == 1)//Set this for boss
        {
            switch (direction)
            {
                case 0:
                    room = Instantiate(BossPrefab, new Vector3(place.x - 25.4f, place.y - 0.6f, place.z + 19.3f), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;

                    break;
                case 1:
                    room = Instantiate(BossPrefab, new Vector3(place.x -35f, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 2:
                    room = Instantiate(BossPrefab, new Vector3(place.x - 16, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 3:
                    room = Instantiate(BossPrefab, new Vector3(place.x - 25.4f, place.y - 0.6f, place.z + 0.7f), Quaternion.identity) as GameObject;
                    room.GetComponent<FirstBossRoom>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
            }
        }
        else
        {

        switch (direction) { 
            case 0:
                room = Instantiate(PrefabList[chosen], new Vector3(place.x - 10.5f, place.y - 0.6f, place.z + 19.3f), Quaternion.identity) as GameObject;
                room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                roomsGenerated++;


                break;
            case 1:
                room = Instantiate(PrefabList[chosen], new Vector3(place.x - 19.5f, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                roomsGenerated++;
                break;
            case 2:
                room = Instantiate(PrefabList[chosen], new Vector3(place.x - 0.5f, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                roomsGenerated++;
                break;
            case 3:
                room = Instantiate(PrefabList[chosen], new Vector3(place.x - 10.5f, place.y - 0.6f, place.z + 0.7f), Quaternion.identity) as GameObject;
                room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                roomsGenerated++;
                break;
            }
                

        }
        //collisionRef.SetActive(false);
    }
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
