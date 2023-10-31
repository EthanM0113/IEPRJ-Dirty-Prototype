using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstBossManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hpTorches;
    public List<int> litTorches;
    private bool isDoneSettingTorches = false;
    bool areBothTorchesLit = false;

    // Boss 
    [SerializeField] private TextMeshProUGUI bossHpText;
    [SerializeField] private FirstBossMovement firstBossMovement;
    [SerializeField] private float speedIncrementOnHit;
    private bool isBossDead = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private float diplayDistance = 10.0f;
    [SerializeField] private FirstBossUIManager firstBossUIManager;
    private float bossMaxHp = 1f;
    private float bossHp = 1f;
    private float bossMaxSpeed;
    private bool isCutscenePlaying = false;
    [SerializeField] private Animator firstBossAnimator;
    [SerializeField] private GameObject attackCircle;
    [SerializeField] private GameObject attackAura;
    [SerializeField] private GameObject flameEffect;
    [SerializeField] private ParticleSystem deathParticles;

    // Boss Hp Bar Variables
    [SerializeField] private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Set Max Values
        bossMaxHp = bossHp;
        bossMaxSpeed = firstBossMovement.GetSpeed();

        //bossHpText.text = bossHp.ToString();

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
                    //bossHpText.text = bossHp.ToString();
                    if (!isBossDead)
                        LigtTwoTorches();
                }
            }
            else
            {
                // Boss Killed
                if(!isCutscenePlaying)
                    StartCoroutine(PlayBossDeathCutscene());
                //DisplayWinScreen();
            }
        }
        
    }

    private IEnumerator PlayBossDeathCutscene()
    {
        Debug.Log("PLAYING BOSS DEATH CUTSCENE");

        isCutscenePlaying = true;

        attackCircle.SetActive(false);  
        attackAura.SetActive(false);    
        firstBossMovement.DisableMovement();
        firstBossUIManager.DisableBossUI();
        flameEffect.SetActive(false);

        yield return new WaitForSeconds(5.0f);
        deathParticles.Play();
        firstBossAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(2.0f); // finish death animation
        firstBossMovement.gameObject.SetActive(false);
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
        bossHp -= 0.34f; // 3 hits to die
        healthBar.fillAmount = bossHp;
        firstBossMovement.IncreaseSpeed(speedIncrementOnHit);
    }

    public bool CheckBossDead()
    {
        if (bossHp <= 0f)
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
