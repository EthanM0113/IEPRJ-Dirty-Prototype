using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] float screenTime = 3.0f;
    bool canClick = false;

    // Start is called before the first frame update
    void Start()
    {
        screenTime = Time.time + screenTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (screenTime < Time.time)
        {
            canClick = true;
        }

        if (canClick)
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
    }
}
