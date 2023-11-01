using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BOSS_STATE
{
    PHASE_ONE,
    PHASE_TWO
};

public class FinalBossManager : MonoBehaviour
{
    // Health Variables
    [SerializeField] private Image hpBar;
    private float maxHP;
    private float currentHP;
    [SerializeField] private BOSS_STATE state;

    // Projectiles
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float orbForce;

    // Shadow Hand
    [SerializeField] private GameObject shadowHandPrefab;

    // Teleproting
    [SerializeField] private GameObject TLBounds;
    [SerializeField] private GameObject BRBounds;
    private Vector3 tlBoundsPos;
    private Vector3 brBoundsPos;
    private Vector3 newPos;
    private int actionsToTeleport;
    private int minTPActions = 2;
    private int maxTPActions = 4;

    // General 
    [SerializeField] private bool doingAction;
    private float actionTicks = 0f;
    private float actionInterval = 1f;
    [SerializeField] private GameObject finalBoss;
    private int chosenAction = 1; // Start with medium projectile attack
    private int actionsDone = 0;
    private float actionDelay = 2f;
    [SerializeField] private GameObject endScreen;

    // Debugging 
    [SerializeField] private bool skip1stPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 1f;
        currentHP = maxHP;
        hpBar.fillAmount = currentHP;
        state = BOSS_STATE.PHASE_ONE;
        doingAction = false;
        tlBoundsPos = TLBounds.gameObject.transform.position;
        brBoundsPos = BRBounds.gameObject.transform.position;
        endScreen.SetActive(false);

        // Set how many actions to be done before teleporting
        actionsToTeleport = Random.Range(minTPActions, maxTPActions + 1);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckTransitionPhase());
        
        if(state == BOSS_STATE.PHASE_ONE && !skip1stPhase)
        {
            if(actionsDone == actionsToTeleport)
            {
                if (!doingAction)
                    StartCoroutine(RandomTeleport());
            }
            else if(chosenAction == 0)
            {
                if (!doingAction)
                    StartCoroutine(ShootSmallProjectiles());
            }
            else if(chosenAction == 1) 
            {
                if (!doingAction)
                    StartCoroutine(ShootMediumProjectiles());
            }
            else if(chosenAction == 2)
            {
                if (!doingAction)
                    StartCoroutine(ShootLargeProjectile());
            }
            else if (chosenAction == 3)
            {
                if (!doingAction)
                    StartCoroutine(SpawnShadowHands());
            }
        }

    }

    private IEnumerator CheckTransitionPhase()
    {
        if(currentHP <= 0f && state == BOSS_STATE.PHASE_ONE)
        {
            yield return new WaitForSeconds(2f); // Wait a sec for suspense
            Debug.Log("1st Phase Done");
            currentHP = 1f; // Fill HP Again
            hpBar.fillAmount = currentHP;
            state = BOSS_STATE.PHASE_TWO;
        }
        else if (currentHP <= 0f && state == BOSS_STATE.PHASE_TWO)
        {
            yield return new WaitForSeconds(2f); // Wait a sec for suspense
            Debug.Log("Beat Final Boss, Congratulations.");
            endScreen.SetActive(true);
        }
    }

    private IEnumerator RandomTeleport()
    {
        doingAction = true;

        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
        float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
        float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
        newPos = new Vector3(newX, finalBoss.transform.position.y, newZ);
        finalBoss.transform.position = newPos;

        // Reset actions
        actionsToTeleport = Random.Range(minTPActions, maxTPActions + 1);
        actionsDone = 0;

        yield return new WaitForSeconds(0.7f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
    }

    private IEnumerator SpawnShadowHands()
    {
        doingAction = true;

        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        for(int i = 0; i < 10; i++)
        {
            float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
            float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
            newPos = new Vector3(newX, shadowHandPrefab.transform.position.y, newZ);

            GameObject shadowHand = Instantiate(shadowHandPrefab, finalBoss.transform);
            shadowHand.transform.parent = null;

            shadowHand.transform.position = newPos;
            yield return new WaitForSeconds(0.1f); // Wait a sec before spawning next
        }


        yield return new WaitForSeconds(2f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
        actionsDone++;
    }


    private IEnumerator ShootSmallProjectiles()
    {
        doingAction = true;

        // Set projectile speed
        orbForce = 100;
        float gapDuration = Random.Range(0f, 0.1f);

        float randomAngle = 0f;
        for (int i = 0; i < 36; i++)
        {
                GameObject orb = Instantiate(orbPrefab, finalBoss.transform);
                orb.transform.parent = null;
                Rigidbody orbRB = orb.GetComponent<Rigidbody>();
                // Just affecting x and y scale
                orb.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);


                Vector3 rightDirection = finalBoss.transform.right;
                Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                orbRB.AddForce(newDirection * orbForce);
            
                yield return new WaitForSeconds(gapDuration); // Enumerator to create gap
                randomAngle += 10f;
        }

        yield return new WaitForSeconds(1f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
        actionsDone++;
    }

    private IEnumerator ShootMediumProjectiles()
    {
        doingAction = true;

        // Set projectile speed
        orbForce = 300f;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject orb = Instantiate(orbPrefab, finalBoss.transform);
                orb.transform.parent = null;
                Rigidbody orbRB = orb.GetComponent<Rigidbody>();
                // Just affecting x and y scale
                orb.transform.localScale = new Vector3(0.5f ,0.5f, 0.5f);

                #region Rotate Each Orb
                if (j == 0)
                {
                    float randomAngle = Random.Range(0f, 89f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 1)
                {
                    float randomAngle = Random.Range(90f, 179f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 2)
                {
                    float randomAngle = Random.Range(180f, 269f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 3)
                {
                    float randomAngle = Random.Range(270f, 359f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                #endregion
            }
            yield return new WaitForSeconds(0.4f); // Wait a sec before shooting
        }

        yield return new WaitForSeconds(1f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
        actionsDone++;
    }

    private IEnumerator ShootLargeProjectile()
    {
        doingAction = true;

        // Set projectile speed
        orbForce = 600f;

        GameObject orb = Instantiate(orbPrefab, finalBoss.transform);
        orb.transform.parent = null;
        Rigidbody orbRB = orb.GetComponent<Rigidbody>();
        // Just affecting x and y scale
        orb.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        // Find Player Location
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = player.transform.position - finalBoss.transform.position;
        directionToPlayer = directionToPlayer.normalized;
        orbRB.AddForce(directionToPlayer * orbForce);


        yield return new WaitForSeconds(2f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
        actionsDone++;
    }

    public void DamageBoss()
    {
        if(state == BOSS_STATE.PHASE_ONE)
        {
            currentHP -= 0.13f; // 8 hits to die
        }
        else if (state == BOSS_STATE.PHASE_TWO)
        {
            currentHP -= 0.09f; // 12 hits to die
        }
        
        hpBar.fillAmount = currentHP;
    }


}
