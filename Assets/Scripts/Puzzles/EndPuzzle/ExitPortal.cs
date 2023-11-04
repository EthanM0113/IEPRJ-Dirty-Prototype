using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    [SerializeField] private GameObject exitPortal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            exitPortal.GetComponent<SceneSelector>().ChangeLevels();
    }
}
