using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartSound : MonoBehaviour
{
    [SerializeField] private AudioClip startSound;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(startSound);
    }
}
