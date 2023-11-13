using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrelPlate : MonoBehaviour
{
    [SerializeField] private GameObject barrel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayInteract();
            barrel.GetComponent<Barrel>().SwitchState();
        }
    }
}
