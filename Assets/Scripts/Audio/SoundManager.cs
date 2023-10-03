using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //audio players
    [SerializeField] private AudioSource musicSource, effectsSource, walkingSource;

    //sound effects
    [SerializeField] private AudioClip playerDeath, fireball, backstabMiss, backstabHit, gameStart, pause, playerWalk, bigTorch;

    //ost
    [SerializeField] private AudioClip darkerTheme, gameOver, levelMusic, mainMenu, torchbearer;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
            //Destroy(gameObject);
    }
    
    public void Torchbearer() {
        musicSource.volume = 0.3f;
        musicSource.PlayOneShot(torchbearer);
        musicSource.loop = true;
    }

    public void MainMenu() {
        musicSource.PlayOneShot(mainMenu);
        musicSource.loop = true;
    }

    public void LevelMusic() {
        musicSource.PlayOneShot(levelMusic);
        musicSource.loop = true;
    }

    public void GameOver() {
        musicSource.PlayOneShot(gameOver);
        musicSource.loop = true;
    }

    public void DarkerTheme() {
        musicSource.PlayOneShot(darkerTheme);
        musicSource.loop = true;
    }

    public void Walk() {
        walkingSource.PlayOneShot(playerWalk);
    }

    public void Torch() {
        effectsSource.volume = 0.8f;
        effectsSource.PlayOneShot(bigTorch);
    }

    public void GameStart() {
        effectsSource.volume = 0.8f;
        effectsSource.PlayOneShot(gameStart);
    }

    public void Fireball() {
        effectsSource.volume = 0.8f;
        effectsSource.PlayOneShot(fireball);
    }

    public void BackstabMiss()
    {
        effectsSource.volume = 0.3f;
        effectsSource.PlayOneShot(backstabMiss);
    }

    public void BackstabHit()
    {
        effectsSource.volume = 0.3f;
        effectsSource.PlayOneShot(backstabHit);
    }

    public void PlayerDeath() {
        effectsSource.volume = 0.8f;
        effectsSource.PlayOneShot(playerDeath);
    }

    public void Pause() {
        effectsSource.volume = 1.0f;
        effectsSource.PlayOneShot(pause);
    }

    public void PlaySound(AudioClip sound) {
        effectsSource.volume = 0.01f;
        effectsSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music) {
        musicSource.PlayOneShot(music);
        musicSource.loop = true;
    }

}
