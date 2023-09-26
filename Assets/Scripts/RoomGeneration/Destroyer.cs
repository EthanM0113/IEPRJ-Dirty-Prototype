using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other != this.gameObject ){
            
            if (!other.CompareTag("Player")) { 
                Destroy(other.gameObject);
                Debug.Log("Delete Trigger");
            }
        }
        
    }
}
