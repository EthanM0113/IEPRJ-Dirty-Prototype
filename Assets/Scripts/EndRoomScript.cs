using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomScript : MonoBehaviour
{
    Collider collider;
    [SerializeField] GameObject uPopup;
    private void Start()
    {
        collider = GetComponent<Collider>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.U))
        {
            SceneSelector sceneSelector = GetComponent<SceneSelector>();
            sceneSelector.ChangeLevels();
        }
        if (other.gameObject.CompareTag("Player") )
        {
            uPopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uPopup.SetActive(false);
        }
    }
}
