using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private Animator anim;
    private string sceneToGoTo;
    [SerializeField] private float loadTime = 10.0f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    [SerializeField] private AudioClip buttonSound;
    public void PlayGame() {
        Debug.Log("PlayGame");
        SoundManager.Instance.PlaySound(buttonSound);
        Time.timeScale = 1;
        sceneToGoTo = "LevelOne";
        anim.SetTrigger("Load");
        //SceneManager.LoadScene("LevelOne");
    }

    public void PlayTutorial()
    {
        Debug.Log("PlayGame");
        SoundManager.Instance.PlaySound(buttonSound);
        Time.timeScale = 1;
        sceneToGoTo = "TutorialLevel";
        anim.SetTrigger("Load");
        //SceneManager.LoadScene("TutorialLevel");
    }

    public void OpenTutorialMenu()
    {
        anim.SetTrigger("OpenTutorial");
    }

    public void ExitGame() {
        Debug.Log("Exited");
        SoundManager.Instance.PlaySound(buttonSound);
        Application.Quit();
    }

    public void StartLevel()
    {
        Invoke("Play", loadTime);
    }

    void Play()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }
}
