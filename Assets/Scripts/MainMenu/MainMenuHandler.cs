using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    public void PlayGame() {
        Debug.Log("PlayGame");
        SoundManager.Instance.PlaySound(buttonSound);
        Time.timeScale = 1;
        SceneManager.LoadScene("TutorialLevel");
    }

    public void ExitGame() {
        Debug.Log("Exited");
        SoundManager.Instance.PlaySound(buttonSound);
        Application.Quit();
    }
}
