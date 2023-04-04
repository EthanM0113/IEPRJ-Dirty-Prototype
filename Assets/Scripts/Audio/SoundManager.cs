using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource musicSource, effectsSource, walkingSource;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        effectsSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.PlayOneShot(music);
        musicSource.loop = true;
    }

    public void PlayWalk(AudioClip sound)
    {
        effectsSource.PlayOneShot(sound);
    }
}
