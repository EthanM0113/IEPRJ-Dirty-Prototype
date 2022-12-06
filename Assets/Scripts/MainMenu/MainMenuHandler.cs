using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame() {
        Debug.Log("PlayGame");
    }

    public void ExitGame() {
        Debug.Log("Exited");
        Application.Quit();
    }
}
