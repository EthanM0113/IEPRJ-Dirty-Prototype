using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossUI;
    [SerializeField] private bool playerWithinRange;
    [SerializeField] private bool bossMusicPlayed;
    private bool isBossDead = false;

    // Start is called before the first frame update
    void Start()
    {
        playerWithinRange = false;
        bossMusicPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayBossUI();
    }

    private void DisplayBossUI()
    {
        if(!isBossDead)
        {
            if (playerWithinRange)
            {
                if (!bossMusicPlayed)
                {
                    //SoundManager.Instance.Torchbearer(); // play music as well
                    bossMusicPlayed = true;
                }

                bossUI.SetActive(true);
            }
            else
            {
                bossUI.SetActive(false);
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.CompareTag("Player"))
        {
            playerWithinRange = true;
            Debug.Log(other.gameObject.name + "entered boss area");
        }
 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerWithinRange = false;
            Debug.Log(other.gameObject.name + "exited boss area");
        }   
    }

    public bool GetPlayerWithinRange()
    {
        return playerWithinRange;
    }

    public void DisableBossUI()
    {
        isBossDead = true;
        bossUI.SetActive(false);    
    }
}
