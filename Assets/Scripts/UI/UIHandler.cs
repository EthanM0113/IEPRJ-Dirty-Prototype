using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject PausePanel;

    [SerializeField] public Slider brightnessSlider;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;
    [SerializeField] private AudioMixer _volumeMixer;
    [SerializeField] private Image _overLay;

    [SerializeField] GameObject winScreen;


    private void Start()
    {
        //_overLay.gameObject.SetActive(true);
        SetSettings();
        winScreen.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void SetSettings() {
        // _volumeMixer.SetFloat("BG_Music", PlayerDataHolder.Instance.GetCurrentVolume());
        SoundManager.Instance.editVolume(PlayerDataHolder.Instance.GetCurrentMusicVolume(), PlayerDataHolder.Instance.GetCurrentSFXVolume());
        musicSlider.value = SoundManager.Instance.GetMusicMultiplier();
        sfxSlider.value = SoundManager.Instance.GetSFXMultiplier();

        brightnessSlider.value = PlayerDataHolder.Instance.GetCurrentColor();
        var tempColor = _overLay.color;
        tempColor.a = 1.0f - brightnessSlider.value;
        _overLay.color = tempColor;
    }

    public void AdjustMusic()
    {
        SoundManager.Instance.editMusic(musicSlider.value); 
    }

    public void AdjustSFX()
    {
        SoundManager.Instance.editSFX(sfxSlider.value);
    }

    public void DarkOverlay()
    {
        var tempColor = _overLay.color;
        tempColor.a = 1.0f - brightnessSlider.value;
        _overLay.color = tempColor;
    }

    public void MainMenu()
    {
        SoundManager.Instance.PlaySound(buttonSound);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void ResumeButton()
    {
        SoundManager.Instance.PlaySound(buttonSound);
        FindObjectOfType<PlayerController>().UnpauseGame();
    }

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySound(buttonSound);
        SettingsPanel.SetActive(true);
        PausePanel.SetActive(false);

    }
    public void CloseSettings()
    {
        SoundManager.Instance.PlaySound(buttonSound);
        SettingsPanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetWinScreenEnabled(bool flag)
    {
        winScreen.SetActive(flag);
    }
}