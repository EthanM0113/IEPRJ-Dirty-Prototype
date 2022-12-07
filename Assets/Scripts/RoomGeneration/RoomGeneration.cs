using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    private bool create = false;
    [SerializeField] GameObject[] PrefabList;
    
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
