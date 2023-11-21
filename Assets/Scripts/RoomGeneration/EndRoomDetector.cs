using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRoomDetector : MonoBehaviour
{
    private bool hasPlayed = false;
    private bool isBossLevel = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelFour" ||
            SceneManager.GetActiveScene().name == "LevelTwo" || SceneManager.GetActiveScene().name == "FinalBossTest")
        {
           isBossLevel = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.parent.GetComponentInParent<RoomProperties>().EndWalls();
            if (isBossLevel && !hasPlayed)
            {
                SoundManager.Instance.DisableDarkerTheme();
                if(SceneManager.GetActiveScene().name == "LevelTwo")
                {
                    SoundManager.Instance.Torchbearer(); // play music as well
                    hasPlayed = true;
                }
                else if(SceneManager.GetActiveScene().name == "LevelFour")
                {
                    SoundManager.Instance.ShadowBoss(); // play music as well
                    hasPlayed = true;
                }
            }

        }
    }
    
}

