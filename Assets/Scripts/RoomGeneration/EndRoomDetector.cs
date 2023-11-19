using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRoomDetector : MonoBehaviour
{
    [SerializeField] private AudioClip BossMusic;
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
            if (isBossLevel)
            {
                SoundManager.Instance.DisableDarkerTheme();
                SoundManager.Instance.PlayMusic(BossMusic);
            }

        }
    }
    
}

