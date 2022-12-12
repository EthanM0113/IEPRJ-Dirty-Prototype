using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    private bool create = false;
    [SerializeField] GameObject[] PrefabList;
<<<<<<< Updated upstream
    
=======
    [SerializeField] private int roomsGenerated = 1;
    [SerializeField] private int roomsTillBoss;
    [SerializeField] private GameObject FirstBossArea;
    //[SerializeField] GameObject footCollider;

    private void OnCollisionStay(Collision collision)
    {
        RoomConditions room;
       if ( room = collision.gameObject.GetComponentInParent<RoomConditions>())
        Debug.Log($"Room ID {room.GetRoomID()}");
    }

>>>>>>> Stashed changes
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NorthWall" )
        {
            CreateRoom(0, other.gameObject);
        }
        else if (other.gameObject.tag == "EastWall")
        {
            CreateRoom(1, other.gameObject);
        }
        else if (other.gameObject.tag == "WestWall")
        {
            CreateRoom(2, other.gameObject);
            create = true;
        }
        else if (other.gameObject.tag == "SouthWall")
        {
            CreateRoom(3, other.gameObject);
        }

    }

    private void CreateRoom (int direction, GameObject collisionRef)
    {
<<<<<<< Updated upstream
        Debug.Log("CREATE ROOM");
        //int chosen = Random.Range(1, PrefabList.Length + 1);
        int chosen = 0;
        Vector3 place = collisionRef.transform.position;
        switch (direction) { 
            case 0:
                Instantiate(PrefabList[chosen], new Vector3(place.x, place.y - 3, place.z + 16), Quaternion.identity);
                break;
            case 1:
                Instantiate(PrefabList[chosen], new Vector3(place.x-8, place.y - 3.5f, place.z + 5), Quaternion.identity);
                break;
            case 2:
                Instantiate(PrefabList[chosen], new Vector3(place.x+8, -2.427262f, place.z + 5), Quaternion.identity);
                break;
            case 3:
                Instantiate(PrefabList[chosen], new Vector3(place.x, place.y - 3, place.z - 16), Quaternion.identity);
                break;
=======
       
        if (roomsGenerated != roomsTillBoss)
        {
            Debug.Log("CREATE ROOM");
            int chosen = Random.Range(0, PrefabList.Length);
            //int chosen = 0;
            GameObject room;
            Vector3 place = collisionRef.transform.position;

            switch (direction)
            {
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
        else // Spawn Boss Area
        {
            Debug.Log("CREATE BOSS ROOM");
            GameObject room;
            Vector3 place = collisionRef.transform.position;
>>>>>>> Stashed changes

            switch (direction)
            {
                case 0:
                    room = Instantiate(FirstBossArea, new Vector3(place.x - 10.5f, place.y - 0.6f, place.z + 19.3f), Quaternion.identity) as GameObject;
                    //room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 1:
                    room = Instantiate(FirstBossArea, new Vector3(place.x - 19.5f, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                    //room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 2:
                    room = Instantiate(FirstBossArea, new Vector3(place.x - 0.5f, place.y - 0.6f, place.z + 10), Quaternion.identity) as GameObject;
                    //room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
                case 3:
                    room = Instantiate(FirstBossArea, new Vector3(place.x - 10.5f, place.y - 0.6f, place.z + 0.7f), Quaternion.identity) as GameObject;
                    //room.GetComponent<RoomConditions>().setup(direction, roomsGenerated);
                    roomsGenerated++;
                    break;
            }
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
