using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.transform.parent.GetComponentInParent<RoomProperties>().EndWalls();
            
        }
    }
}

