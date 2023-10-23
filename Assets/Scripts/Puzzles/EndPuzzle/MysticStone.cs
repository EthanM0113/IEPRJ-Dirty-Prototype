using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticStone : MonoBehaviour
{
    [SerializeField] private GameObject stone; 
    
    void Awake()
    {
        stone.GetComponent<Renderer>().material.color = Color.gray;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            stone.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.gameObject.tag == "Player")
        {
            stone.GetComponent<Renderer>().material.color = Color.gray;
        }
    }
}
