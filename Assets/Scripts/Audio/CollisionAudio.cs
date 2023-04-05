using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    [SerializeField] private GameObject punch;

    void OnCollisionEnter()
    {
        if(!punch.GetComponent<AudioSource>().isPlaying)
            punch.GetComponent<AudioSource>().Play();
    }
}
