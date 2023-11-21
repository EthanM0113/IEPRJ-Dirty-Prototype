using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossUI;
    [SerializeField] private bool playerWithinRange;
    private bool isBossDead = false;

    // Start is called before the first frame update
    void Start()
    {
        playerWithinRange = false;
        //bossMusicPlayed = false;
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
                bossUI.SetActive(true);
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
