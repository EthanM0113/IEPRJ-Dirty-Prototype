using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstBossManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hpTorches;
    public List<int> litTorches;
    private bool isDoneSettingTorches = false;
    bool areBothTorchesLit = false;

    // Boss 
    [SerializeField] private TextMeshProUGUI bossHpText;
    [SerializeField] private int bossHp;
    [SerializeField] private FirstBossMovement firstBossMovement;
    [SerializeField] private float speedIncrementOnHit;
    private bool isBossDead = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private float diplayDistance = 10.0f;
    [SerializeField] private FirstBossUIManager firstBossUIManager;
    private int bossMaxHp;
    private float bossMaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Set Max Values
        bossMaxHp = bossHp;
        bossMaxSpeed = firstBossMovement.GetSpeed();

        bossHpText.text = bossHp.ToString();

        for (int i = 0; i < hpTorches.Count; i++)
        {
            hpTorches[i].GetComponent<HpTorchHandler>().SetFlameLight(false);
        }

        LigtTwoTorches();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstBossUIManager.GetPlayerWithinRange()) // should offload some memory
        {
            if (!isBossDead)
            {
                if (areBothTorchesLit = CheckTwoTorchesLit())
                {
                    //Debug.Log("Torches remain lit");
                }
                else
                {
                    //Debug.Log("TAKE DAMAGE!!");
                    DealDamageToBoss();
                    isBossDead = CheckBossDead();
                    bossHpText.text = bossHp.ToString();
                    if (!isBossDead)
                        LigtTwoTorches();
                }
            }
            else
            {
                DisplayWinScreen();
            }
        }
        
    }

    private bool CheckTwoTorchesLit()
    {
        if (hpTorches[litTorches[0]].GetComponent<HpTorchHandler>().GetFlameLight().activeSelf ||
            hpTorches[litTorches[1]].GetComponent<HpTorchHandler>().GetFlameLight().activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LigtTwoTorches()
    {
        litTorches = new List<int>();

        while (!isDoneSettingTorches)
        {
            int pickedTorch = Random.Range(0, 4);
            //Debug.Log("Picked Torch " + pickedTorch);

            if (litTorches.Count == 0)
            {
                litTorches.Add(pickedTorch);
            }
            else
            {
                if (pickedTorch != litTorches[0])
                {
                    litTorches.Add(pickedTorch);
                }
            }

            if (litTorches.Count == 2)
            {
                isDoneSettingTorches = true;
                //Debug.Log("Done Setting Torches");
            }
        }

        //Debug.Log("Selected Torch " + litTorches[0] + " and Torch " + litTorches[1]);

        // Light 2 Selected Torches
        hpTorches[litTorches[0]].GetComponent<HpTorchHandler>().SetFlameLight(true);
        hpTorches[litTorches[1]].GetComponent<HpTorchHandler>().SetFlameLight(true);

        // reset values
        isDoneSettingTorches = false;
    }

    public void DealDamageToBoss()
    {
        bossHp--;
        firstBossMovement.IncreaseSpeed(speedIncrementOnHit);
    }

    public bool CheckBossDead()
    {
        if (bossHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisplayWinScreen()
    {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void ResetValues()
    {
        bossHp = bossMaxHp;
        bossHpText.text = bossHp.ToString();
        firstBossMovement.SetSpeed(bossMaxSpeed);
        for (int i = 0; i < hpTorches.Count; i++)
        {
            hpTorches[i].GetComponent<HpTorchHandler>().SetFlameLight(false);
        }
        LigtTwoTorches();
    }
}
