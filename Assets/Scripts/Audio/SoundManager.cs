using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //audio players
    [SerializeField] private AudioSource musicSource, effectsSource, walkingSource;

    //sound effects
    [SerializeField] private AudioClip playerDeath, fireball, backstabMiss, backstabHit, gameStart, pause, playerWalk, bigTorch, 
        purchaseSuccess, purchaseFail, bossSlay, wispTP, emptyFeedback, detected, interact, doorOpen, propBreak;


    // Torchbearer SFX
    [SerializeField] private AudioClip tb_litTorch, tb_extinguishTorch, tb_gruntA, tb_gruntB, tb_damage, tb_defeat;

    //ost
    [SerializeField] private AudioClip darkerTheme, gameOver, levelMusic, mainMenu, torchbearer;

    // multiplier for option screen
    [SerializeField] private float musicMultiplier = 5f;
    [SerializeField] private float sfxMultiplier = 1.0f;
    [SerializeField] private float walkSFXMultiplier = 1.0f;

    bool enableDarkerMusic = true;
    bool playGameOverOnce = false;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
            //Destroy(gameObject);
    }

    private void Start()
    {
        PlayerDataHolder data = PlayerDataHolder.Instance;

        musicMultiplier = data.GetCurrentMusicVolume();
        sfxMultiplier = data.GetCurrentSFXVolume();
    }

    public void editVolume(float newMusicMultiplier, float newSfxMultiplier)
    {
        //musicSource.volume = music;
        //effectsSource.volume = effect;

        musicMultiplier = newMusicMultiplier;
        sfxMultiplier = newSfxMultiplier;
        walkSFXMultiplier = newSfxMultiplier;

        // For any currently playing music
        musicSource.volume = 10.0f * musicMultiplier;
    }
    
    // OST and Music

    public void Torchbearer() {
        musicSource.volume = 0.1f * musicMultiplier;
        musicSource.PlayOneShot(torchbearer);
        musicSource.loop = true;
    }

    public void MainMenu() {
        musicSource.volume = 0.1f * musicMultiplier;
        musicSource.PlayOneShot(mainMenu);
        musicSource.loop = true;
    }

    public void LevelMusic() {
        musicSource.volume = musicMultiplier;
        musicSource.PlayOneShot(levelMusic);
        musicSource.loop = true;
    }

    public void GameOver() {
        if(playGameOverOnce) return;
        musicSource.volume = .05f * musicMultiplier;
        musicSource.clip = gameOver;
        musicSource.Play();
        musicSource.loop = true;
        playGameOverOnce = true;
    }

    public void DarkerTheme() {
        if (!enableDarkerMusic) return;
        musicSource.volume = 0.1f * musicMultiplier;
        musicSource.PlayOneShot(darkerTheme);
        musicSource.loop = true;
    }

    public void DisableDarkerTheme()
    {
        enableDarkerMusic = false;
    }

    // SFX

    public void Walk() {
        walkingSource.volume = 0.1f * walkSFXMultiplier;
        walkingSource.PlayOneShot(playerWalk);
    }

    public void Torch() {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(bigTorch);
    }

    public void GameStart() {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(gameStart);
    }

    public void Fireball() {
        effectsSource.volume = 0.8f * sfxMultiplier; 
        effectsSource.PlayOneShot(fireball);
    }

    public void EmptySFX()
    {
        effectsSource.volume = 0.4f * sfxMultiplier;
        effectsSource.PlayOneShot(emptyFeedback);
    }

    public void PlayDetected()
    {
        effectsSource.volume = 0.3f * sfxMultiplier;
        effectsSource.PlayOneShot(detected);
    }

    public void PlayInteract()
    {
        effectsSource.volume = 0.4f * sfxMultiplier;
        effectsSource.PlayOneShot(interact);
    }

    public void BackstabMiss()
    {
        effectsSource.volume = 0.3f * sfxMultiplier;
        effectsSource.PlayOneShot(backstabMiss);
    }

    public void BackstabHit()
    {
        effectsSource.volume = sfxMultiplier;
        effectsSource.PlayOneShot(backstabHit);
    }

    public void EnemyPerish(AudioClip baseEnemyPerish, float volume)
    {
        effectsSource.volume = volume * sfxMultiplier; // 0.6f default
        effectsSource.PlayOneShot(baseEnemyPerish);
    }

    public void PurchaseSuccess()
    {
        effectsSource.volume = 1.0f * sfxMultiplier;
        effectsSource.PlayOneShot(purchaseSuccess);
    }

    public void PurchaseFail()
    {
        effectsSource.volume = 1.0f * sfxMultiplier;
        effectsSource.PlayOneShot(purchaseFail);
    }

    public void StaggeredShiv()
    {
        effectsSource.volume = 0.7f * sfxMultiplier;
        effectsSource.PlayOneShot(purchaseFail);
    }

    public void BossSlay()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(bossSlay);
    }

    public void PlayerDeath() {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(playerDeath);
    }
    public void WispTP()
    {
        effectsSource.volume = 0.1f * sfxMultiplier;
        effectsSource.PlayOneShot(wispTP);
    }
    public void TutorialDoorOpen()
    {
        effectsSource.volume = 1.0f * sfxMultiplier;
        effectsSource.PlayOneShot(doorOpen);
    }

    public void PropBreak()
    {
        effectsSource.volume = 0.2f * sfxMultiplier;
        effectsSource.PlayOneShot(propBreak);
    }
    // Torchbearer SFX

    public void TB_LightTorch()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_litTorch);
    }

    public void TB_ExtinguishTorch()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_extinguishTorch);
    }

    public void TB_GruntA()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_gruntA);
    }

    public void TB_GruntB()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_gruntB);
    }

    public void TB_Damage()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_damage);
    }

    public void TB_Slay()
    {
        effectsSource.volume = 0.8f * sfxMultiplier;
        effectsSource.PlayOneShot(tb_defeat);
    }

    public void Pause() {
        effectsSource.volume = 1.0f * sfxMultiplier;
        effectsSource.PlayOneShot(pause);
    }

    public void PlaySound(AudioClip sound) {
        effectsSource.volume = 0.01f * sfxMultiplier;
        effectsSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music) {
        musicSource.clip = music;
        musicSource.Play();
        musicSource.loop = true;
    }

    public float GetMusicMultiplier()
    {
        return musicMultiplier;
    }

    public float GetSFXMultiplier()
    {
        return sfxMultiplier;
    }
    public bool GetIsWalkingSourcePlaying()
    {
        return walkingSource.isPlaying;
    }
}
