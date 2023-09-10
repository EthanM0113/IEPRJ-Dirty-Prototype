using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartSound : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.GameStart();
    }
}
