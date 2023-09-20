using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiDuplicate : MonoBehaviour
{
    BoxCollider col;
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        GameObject wall = other.gameObject;
        if (wall.CompareTag("NorthWall") && wall != this.gameObject)
        {
            Debug.Log("Duplicate Detected");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if (wall.CompareTag("EastWall") && wall != this.gameObject)
        {
            Debug.Log("Duplicate Detected");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if (wall.CompareTag("WestWall") && wall != this.gameObject)
        {

            Debug.Log("Duplicate Detected");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if (wall.CompareTag("SouthWall") && wall != this.gameObject)
        {

            Debug.Log("Duplicate Detected");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
