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


    private void Start()
    {
        SetSettings();
    }

    private void Update()
    {
        
    }

    private void SetSettings() {
        // _volumeMixer.SetFloat("BG_Music", PlayerDataHolder.Instance.GetCurrentVolume());
        SoundManager.Instance.editVolume(PlayerDataHolder.Instance.GetCurrentMusicVolume(), PlayerDataHolder.Instance.GetCurrentSFXVolume());
        var tempColor = _overLay.color;
        tempColor.a = 1.0f - PlayerDataHolder.Instance.GetCurrentColor();
        _overLay.color = tempColor;
    }

    public void AdjustVolume()
    {
        SoundManager.Instance.editVolume(musicSlider.value, sfxSlider.value);
        
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
        PausePanel.SetActive(false);
    }
    
}