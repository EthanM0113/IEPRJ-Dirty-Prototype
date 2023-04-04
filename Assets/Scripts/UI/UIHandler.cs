using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

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
}