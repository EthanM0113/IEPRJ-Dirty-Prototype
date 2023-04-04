using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
