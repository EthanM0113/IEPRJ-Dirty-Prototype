using System;
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
    [SerializeField] private Image delayedHealthBar;
    private float lerpSpeed = 0.01f;

    // Groan 
    private float groanTicks = 0f;
    private float groanInterval = 7f;

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

            EaseHealthBar();

            if (!isBossDead)
            {
                PlayRandomGroans();

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

    private void EaseHealthBar()
    {
        if(healthBar.fillAmount != delayedHealthBar.fillAmount)
        {
            delayedHealthBar.fillAmount = Mathf.Lerp(delayedHealthBar.fillAmount, bossHp, lerpSpeed);
        }
    }

    private void PlayRandomGroans()
    {
        groanTicks += Time.deltaTime;   
        if(groanTicks > groanInterval) 
        {
            int chosenGroan = UnityEngine.Random.Range(0, 2);
            if (chosenGroan == 0)
                SoundManager.Instance.TB_GruntA();
            else if (chosenGroan == 1)
                SoundManager.Instance.TB_GruntB();

            groanTicks = 0;

            groanInterval = UnityEngine.Random.Range(5f, 10f);
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
        SoundManager.Instance.TB_Slay();
        firstBossAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(2.0f); // finish death animation
        firstBossMovement.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f); // Wait for suspense

        // Reward with money
        SoundManager.Instance.BackstabHit();
        PlayerMoneyUIHandler playerMoneyUIHandler = FindObjectOfType<PlayerMoneyUIHandler>();
        playerMoneyUIHandler.SpinCoinImage();
        playerMoneyUIHandler.PulseCointText();
        PlayerMoneyManager.Instance.AddCoins(40); // enough for two upgrades to incentivize completing the puzzle        

        // Add skill slot
        FindObjectOfType<PlayerAbilityHandler>().AddSkillSlot();

        // Play Particle
        firstBossMovement.PlayParticle(GameObject.FindGameObjectWithTag("Player").transform.position);

        Invoke("ChangeLevel", 3f);
        
    }
    
    private void ChangeLevel()
    {
        // Transition to next Level
        SceneSelector sceneSelector = GetComponent<SceneSelector>();
        sceneSelector.ChangeLevels();
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
            int pickedTorch = UnityEngine.Random.Range(0, 4);
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
        SoundManager.Instance.TB_LightTorch();
        hpTorches[litTorches[0]].GetComponent<HpTorchHandler>().SetFlameLight(true);
        hpTorches[litTorches[1]].GetComponent<HpTorchHandler>().SetFlameLight(true);

        // reset values
        isDoneSettingTorches = false;
    }

    public void DealDamageToBoss()
    {
        SoundManager.Instance.TB_Damage();
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
