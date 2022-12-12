using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame() {
        Debug.Log("PlayGame");
        SceneManager.LoadScene("RoomGeneration");
    }

    public void ExitGame() {
        Debug.Log("Exited");
        Application.Quit();
    }
}
