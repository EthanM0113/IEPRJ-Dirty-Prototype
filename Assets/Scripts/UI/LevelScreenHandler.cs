using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class LevelScreenHandler : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject panelToHide;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        anim = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "LevelOne")
        {
            anim.SetTrigger("LevelOne");
        }
        else if (SceneManager.GetActiveScene().name == "LevelTwo")
        {
            anim.SetTrigger("LevelTwo");
        }
        else if (SceneManager.GetActiveScene().name == "LevelThree")
        {
            anim.SetTrigger("LevelThree");
        }
        else if (SceneManager.GetActiveScene().name == "LevelFour")
        {
            anim.SetTrigger("LevelFour");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosePanel()
    {
        Time.timeScale = 1.0f;
        panelToHide.SetActive(false);
    }
}
