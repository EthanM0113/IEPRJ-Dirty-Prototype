using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleRoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject levelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GameObject spawnedLevel = GameObject.Instantiate(levelPrefab);
        }
    }
}
