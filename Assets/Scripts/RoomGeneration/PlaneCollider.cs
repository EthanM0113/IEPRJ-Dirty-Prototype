using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{

    
    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.collider.CompareTag("DeadTestEnemy"))
        {
            this.transform.parent.GetComponent<RoomConditions>().enemyKill();
        }
    }

   
}





