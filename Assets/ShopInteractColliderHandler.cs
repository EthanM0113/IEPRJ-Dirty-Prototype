using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractColliderHandler : MonoBehaviour
{
    [SerializeField] private bool isPlayerWithinRange;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerWithinRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerWithinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerWithinRange = false;
    }

    public bool GetIsPlayerWithinRange()
    {
        return isPlayerWithinRange;
    }
}
