using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOptimization : MonoBehaviour
{
    [SerializeField] GameObject MapNode;
    
    [SerializeField] bool NeedsIcon = false;
    [SerializeField] private bool HasPlayer = false;
    [SerializeField] private bool IsActive = false;
    private bool HasExplored = false;


    [SerializeField] protected List<GameObject> AdjRoomsA;
    // Start is called before the first frame update
    void Start()
    {
        AdjRoomsA = new List<GameObject>();
       
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //if (!IsActive) {
                HasPlayer = true;
                IsActive = true;
                ActivateRooms();
                GameObject.FindGameObjectWithTag("MapNode").transform.position = MapNode.transform.position;
                if (!HasExplored && NeedsIcon)
            {
                MapNode.transform.GetChild(0).gameObject.SetActive(true);
            }

            //}
            //else if (IsActive) //Moving to adjacent room
            //{
            //    HasPlayer = true;
            //    IsActive = true;
            //}
            
        }

        if (collision.gameObject.CompareTag("RoomCollider") || collision.gameObject.CompareTag("StartingRoom"))
        {
            Debug.Log("Room Detected");
            AdjRoomsA.Add(collision.gameObject);
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoomCollider") || collision.gameObject.CompareTag("StartingRoom"))
        {
            Debug.Log("Room Detected");
        }
        if (collision.gameObject.CompareTag("RoomCollider") ||collision.gameObject.CompareTag("StartingRoom"))
        {//Activate the room the player is currently in
            if (HasPlayer && collision.gameObject.GetComponent<RoomOptimization>().IsActive == false)
            {
                collision.gameObject.GetComponent<RoomOptimization>().ActivateRoom();

            }
            
            else if (!IsActive && collision.gameObject.GetComponent<RoomOptimization>().IsActive == true)
            {
                collision.gameObject.GetComponent<RoomOptimization>().DeactivateRoom();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsActive)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                HasPlayer = false;
                for (int i = 0; i < AdjRoomsA.Count - 1; i++) { 
                    //Checks which room the player went to
                    if (AdjRoomsA[i].GetComponent<RoomOptimization>().HasPlayer == true)
                    {

                    }
                }
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("RespawnNode").transform.position = collision.gameObject.transform.position;
        }

    }
   
    void ActivateRooms()
    {
        for (int i = 0; i < AdjRoomsA.Count - 1; i++)
        {
            AdjRoomsA[i].gameObject.GetComponent<RoomOptimization>().ActivateRoom();
        }
    }

    void DeactivateRooms(List<GameObject> AdjRoomsB)
    {
        List<GameObject> AdjRooms = new List<GameObject>();
        //AdjRoomsB is the list of adjacent rooms of the room the player has moved to. AdjRoomsA is the list of adjacent rooms of the room the player has left
        for (int i = 0; i < AdjRoomsA.Count - 1; i++)
        {
            bool isAdjacent = false;
            for (int j = 0; j < AdjRoomsB.Count - 1; j++)
            {
                if (AdjRooms[i] == AdjRoomsB[j]) //Room in between Room A and B
                {
                    isAdjacent = true;
                }
            }
            if (isAdjacent)
            {
                AdjRooms.Add(AdjRoomsA[i]);
            }
            
        }
        for (int i = 0;i < AdjRooms.Count - 1; i++)
        {
            AdjRooms[i].GetComponent<RoomOptimization>().DeactivateRoom();
        }
    }

    public List<GameObject> GetAdjRooms()
    {
        return AdjRoomsA;
    }
    public void ActivateRoom()
    {
        IsActive = true;
    }

    public void DeactivateRoom()
    {
        IsActive = false;
    }    
}
