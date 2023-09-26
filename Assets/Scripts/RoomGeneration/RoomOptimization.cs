using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOptimization : MonoBehaviour
{
    [SerializeField] private bool HasPlayer = false;
    [SerializeField] private bool IsActive = false;

    [SerializeField] private List<GameObject> AdjRooms;
    // Start is called before the first frame update
    void Start()
    {
        AdjRooms = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HasPlayer)
        {

        }

        else if (!HasPlayer && IsActive)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!IsActive) {
                HasPlayer = true;
                IsActive = true;
                ActivateRooms();
            }
            else if (IsActive) //Moving to adjacent room
            {
                HasPlayer = true;
                IsActive = true;
            }
            
        }

        if (collision.gameObject.CompareTag("RoomCollider"))
        {
            AdjRooms.Add(collision.gameObject);
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoomCollider"))
        {
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
            }
        }

    }
   
    void ActivateRooms()
    {
        for (int i = 0; i < AdjRooms.Count - 1; i++)
        {
            AdjRooms[i].gameObject.GetComponent<RoomOptimization>().ActivateRoom();
        }
    }

    void DeactivateRooms(List<GameObject> AdjRoomsB)
    {
        List<GameObject> AdjRoomsA = new List<GameObject>();
        AdjRoomsA = AdjRooms;
        for (int i = 0; i < AdjRoomsA.Count - 1; i++)
        {
            bool isAdjacent = false;
            for (int j = 0; j < AdjRoomsB.Count - 1; j++)
            {
                if (AdjRoomsA[i] == AdjRoomsB[j]) //Room in between Room A and B
                {
                    AdjRoomsA.RemoveAt(i);
                }
            }
            AdjRooms[i].gameObject.GetComponent<RoomOptimization>().DeactivateRoom();
        }
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
