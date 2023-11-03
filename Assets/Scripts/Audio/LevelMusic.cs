using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        musicSource.volume = 0.1f * SoundManager.Instance.GetMusicMultiplier();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
