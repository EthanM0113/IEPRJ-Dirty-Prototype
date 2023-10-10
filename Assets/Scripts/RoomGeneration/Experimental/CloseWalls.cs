using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWalls : MonoBehaviour
{
    [SerializeField] int ClosingDirection;
    //1 needs a bottom door
    //2 needs a top door
    //3 needs a left door
    //4 needs a right door
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomCollider"))
        {
            other.transform.parent.gameObject.GetComponent<RoomProperties>().CloseWalls(ClosingDirection);
        }
    }
}
