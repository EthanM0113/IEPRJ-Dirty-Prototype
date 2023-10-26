using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HelpfulChair : MonoBehaviour
{
    [SerializeField] private GameObject chair;
    [SerializeField] private GameObject playerInventory;

    private bool obtained = false;


    public void TakeScroll()
    {
        if (obtained)
            return;

        playerInventory.GetComponent<PlayerInventory>().SetFreezingScroll(true);
        chair.GetComponent<LoreObject>().UpdateText("Obtained a Scroll of Freezing!");
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
            chair.GetComponent<LoreObject>().SetText("It's just a regular chair now, like the rest.");
    }
}
