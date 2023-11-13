using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    int numberOfHands = 20;

    // Teleporting
    [SerializeField] private GameObject TLBounds;
    [SerializeField] private GameObject BRBounds;
    private Vector3 tlBoundsPos;
    private Vector3 brBoundsPos;
    private Vector3 newPos;
    [SerializeField] private int actionsToTeleport;
    private int minTPActions = 2;
    private int maxTPActions = 4;

    // General 
    [SerializeField] private bool doingAction;
    [SerializeField] private GameObject finalBoss;
    private int chosenAction = 1; // Start with medium projectile attack
    [SerializeField] private int actionsDone = 0;
    [SerializeField] private GameObject endScreen;
    private bool isInvincible = false;
    private FinalBossUIManager finalBossUIManager;
    [SerializeField] private Animator finalBossAnimator;
    private bool isFacingLeft = true;
    private SpriteRenderer finalBossSpriteRenderer;
    [SerializeField] private GameObject puddleCollider;
    [SerializeField] private ParticleSystem hitParticles;
    private bool isRooted = false;
    [SerializeField] private Collider firstBossCollider;

    // Phase 2 
    private bool isTransitioning = false;
    private bool isHunting = false;
    private float randomAttackTicks = 0f;
    private float randomAttackInterval = 2f;
    [SerializeField] private float huntSpeed;
    private float originalHuntSpeed = 2f;
    [SerializeField] private GameObject meleeAreaPrefab;
    private bool isMelee = false;
    private int meleesDone = 0;
    private int meleesRequired = 1; // at first do 1 melee before switching to shadow form


    // Roaming 
    private Vector3 roamPosition = Vector3.zero;
    private bool isRoaming;
    private float roamTicks = 0f;
    [SerializeField] private float roamDuration;

    // Debugging 
    [SerializeField] private bool skip1stPhase = false;

    // Player Variables
    private PlayerHearts playerHearts;

    // Audio
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip SFX_Phase1Cast;
    [SerializeField] private AudioClip SFX_Phase1Channel;
    [SerializeField] private AudioClip SFX_Phase1Defeat;
    [SerializeField] private AudioClip SFX_Phase1Idle;
    [SerializeField] private AudioClip SFX_ShadowHand;

    [SerializeField] private AudioClip SFX_Phase2TransitionRoar;
    [SerializeField] private AudioClip SFX_Phase2Melee;
    [SerializeField] private AudioClip SFX_Phase2Move;
    [SerializeField] private AudioClip SFX_Phase2Puddle;
    private bool isPuddleSFXPlayed = false;
    [SerializeField] private AudioClip SFX_Phase2DeathRoar;
    private bool isDeathRoarSFXPlayed = false;

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
        puddleCollider.SetActive(false);

        // Set how many actions to be done before teleporting
        actionsToTeleport = Random.Range(minTPActions, maxTPActions + 1);

        finalBossUIManager = FindObjectOfType<FinalBossUIManager>();

        huntSpeed = originalHuntSpeed;

        // Get Sprite Renderer
        finalBossSpriteRenderer = finalBoss.GetComponentInChildren<SpriteRenderer>();

        //  Find Player Variables
        playerHearts = GameObject.FindObjectOfType<PlayerHearts>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finalBossUIManager.GetPlayerWithinRange())
        {
            if(currentHP > 0f)
            FlipBoss();

            StartCoroutine(CheckTransitionPhase());

            if (state == BOSS_STATE.PHASE_ONE && !skip1stPhase && !isTransitioning)
            {
                if(!isRooted)
                {
                    if (actionsDone >= actionsToTeleport)
                    {
                        if (!doingAction)
                            StartCoroutine(RandomTeleport());
                    }
                    else if (chosenAction == 0)
                    {
                        if (!doingAction)
                            StartCoroutine(ShootSmallProjectiles());
                    }
                    else if (chosenAction == 1)
                    {
                        if (!doingAction)
                            StartCoroutine(ShootMediumProjectiles());
                    }
                    else if (chosenAction == 2)
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
            else if (state == BOSS_STATE.PHASE_TWO && currentHP > 0f && !isTransitioning)
            {
                if(!isRooted) 
                {
                    RandomBossAttack();
                }
                
                if (isHunting)
                {
                    if (!isRooted)
                    {
                        StartCoroutine(CheckMeleeAttack());
                        HuntPlayer();
                    }  
                }
                else if (!isHunting)
                {
                    // Transform into shadow and roam around room
                    StartCoroutine(TriggerShadowForm());
                }
            }
            
        }
    }

    private void FlipBoss()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 difference = finalBoss.transform.position - player.transform.position;

        if(difference.x < 0f) // Face Right
        {
            if(isFacingLeft)
            {
                finalBossSpriteRenderer.flipX = true;
                isFacingLeft = false;
            }
        }
        else if(difference.x >= 0f) // Face Left
        {
            if (!isFacingLeft)
            {
                finalBossSpriteRenderer.flipX = false;
                isFacingLeft = true;
            }
        }
    }

    private IEnumerator TriggerShadowForm()
    {
        // Track time passed while roaming, after x time go back to hunting
        roamTicks += Time.deltaTime;
        if (roamTicks >= roamDuration)
        {
            // Transform back to Idle from Puddle
            huntSpeed = originalHuntSpeed;
            finalBossAnimator.SetBool("isPuddle", false);
            finalBossAnimator.SetBool("isPuddleToIdle", true);

            // Play Puddle SFX
            if(!isPuddleSFXPlayed)
            {
                sfxSource.PlayOneShot(SFX_Phase2Puddle);
                isPuddleSFXPlayed = true;
            }

            yield return new WaitForSeconds(0.8f); // Finish Animation  
            puddleCollider.SetActive(false);
            finalBossAnimator.SetBool("isPuddleToIdle", false);

            isInvincible = false; // make vulnerable already as soon as reaching target duration and animation finished
            yield return new WaitForSeconds(3f); // Wait a sec for VERY vulnerable
            roamTicks = 0f;
            isRoaming = false;
            isHunting = true;
            isPuddleSFXPlayed = false;
        }

        if(!isHunting)
        {
            // Find new roam position
            if (!isRoaming)
            {
                Random.InitState(Random.Range(int.MinValue, int.MaxValue));
                float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
                float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
                roamPosition = new Vector3(newX, finalBoss.transform.position.y, newZ);
                isRoaming = true;
                isInvincible = true;
            }

            // Move towards roam position
            huntSpeed = originalHuntSpeed + 3.5f;
            finalBoss.transform.position = Vector3.MoveTowards(finalBoss.transform.position, roamPosition, huntSpeed * Time.deltaTime);

            // Check distance to roam position
            float distance = Vector3.Distance(finalBoss.transform.position, roamPosition);

            // Find new roam position upon reaching or after certain time passes
            if (distance <= 0.01f)
            {
                isRoaming = false;
            }
        }  
    }

    private IEnumerator CheckMeleeAttack()
    {
   
        // Find Player Location
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check Distance to Player
        float distance = Vector3.Distance(finalBoss.transform.position, player.transform.position);

        // Melee if Close enough
        if (distance < 1f && !isMelee)
        {
            finalBossAnimator.SetBool("isPuddleToIdle", false);
            finalBossAnimator.SetBool("isHunting", false);
            finalBossAnimator.SetBool("isMelee", true);
            isMelee = true;
            huntSpeed = 0; // stop movement

            GameObject meleeArea = Instantiate(meleeAreaPrefab, player.transform);
            meleeArea.transform.parent = null;
            meleeArea.transform.localScale = new Vector3(3f, 3f, 3f);

            yield return new WaitForSeconds(0.4f); // Play Melee SFX on Slam
            sfxSource.PlayOneShot(SFX_Phase2Melee);

            yield return new WaitForSeconds(0.4f); // Finish Animation
            
            finalBossAnimator.SetBool("isMelee", false);
            yield return new WaitForSeconds(2f); // Wait a sec for vulnerable
            huntSpeed = originalHuntSpeed;

            meleesDone++;
            // Roam or turn into shadow form after certain melee attacks
            if (meleesDone == meleesRequired)
            {
                meleesRequired = Random.Range(1, 4);
                meleesDone = 0;
                roamTicks = 0f;

                // Transition to shadow form
                isInvincible = true;
                puddleCollider.SetActive(true);
                finalBossAnimator.SetBool("isIdleToPuddle", true);

                // Play Puddle SFX
                sfxSource.PlayOneShot(SFX_Phase2Puddle);

                yield return new WaitForSeconds(0.9f); // Finish Animation
                finalBossAnimator.SetBool("isIdleToPuddle", false);
                finalBossAnimator.SetBool("isPuddle", true);

                isHunting = false;
            }
            isMelee = false;
        }
        
    }

    private void RandomBossAttack()
    {
        randomAttackTicks += Time.deltaTime;
        if(randomAttackTicks > randomAttackInterval)
        {
            int chosenAttack = Random.Range(0, 2);
            if(chosenAttack == 0)
            {
                if (!doingAction)
                {
                    StartCoroutine(SpawnShadowHands());
                }
                    
            }
            if (chosenAttack == 1)
            {
                if (!doingAction)
                {
                    StartCoroutine(ShootMediumProjectiles());
                }
            }

            randomAttackTicks = 0f;
            randomAttackInterval = Random.Range(2f, 4f);
        }
    }

    private void HuntPlayer()
    {
        // Find Player Location
        if(!isMelee && isHunting)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            finalBoss.transform.position = Vector3.MoveTowards(finalBoss.transform.position, player.transform.position, huntSpeed * Time.deltaTime);
            finalBossAnimator.SetBool("isHunting", true);
            // Play Move SFX
            //sfxSource.PlayOneShot(SFX_Phase2Move);
        }
    }

    private IEnumerator CheckTransitionPhase()
    {
        if(currentHP <= 0f && state == BOSS_STATE.PHASE_ONE && !isTransitioning)
        {
            TurnOffAllPhase1Animations();

            isTransitioning = true;
            isInvincible = true;

            doingAction = true;

            // Play Phase 1 Death Sounds Sounds
            //sfxSource.PlayOneShot(SFX_Phase1Defeat);

            // Play Death Animation
            finalBossAnimator.SetBool("isPhase1Done", true);
            yield return new WaitForSeconds(3f); // Play Roar SFX at certain point
            // Play Roar SFX
            sfxSource.PlayOneShot(SFX_Phase2TransitionRoar);
            yield return new WaitForSeconds(1.4f); // Let full animation finish
            finalBossAnimator.SetBool("isPhase1Done", false);

            Debug.Log("1st Phase Done");
            currentHP = 1f; // Fill HP Again
            hpBar.fillAmount = currentHP;

            // Switch to Phase 2 Animation
         
            finalBossAnimator.SetBool("isPhase2", true);

            // Modify Phase 2 Collider
            firstBossCollider.GetComponent<BoxCollider>().center = new Vector3(-0.6545104f, 0.5459307f, -0.4049391f);
            firstBossCollider.GetComponent<BoxCollider>().size = new Vector3(3.67502f, 2.12686f, 1.83501f);

            // Set Phase 2 Variables
            doingAction = false;
            isInvincible = false;
            isTransitioning = false;
            isHunting = true;
            isRoaming = false;
            state = BOSS_STATE.PHASE_TWO;
            
        }
        else if (currentHP <= 0f && state == BOSS_STATE.PHASE_TWO)
        {
            finalBossUIManager.DisableHPUI();
            TurnOffAllPhase2Animations();
            yield return new WaitForSeconds(1f); // Wait a sec for suspense
            // Play Death SFX
            if (!isDeathRoarSFXPlayed)
            {
                sfxSource.PlayOneShot(SFX_Phase2DeathRoar);
                isDeathRoarSFXPlayed = true;
            }
            yield return new WaitForSeconds(1f); // Wait a sec for suspense
            finalBossAnimator.SetBool("isDead", true);

   

            yield return new WaitForSeconds(3f); // Finsh Animation + a bit more
            Debug.Log("Beat Final Boss, Congratulations.");
            endScreen.SetActive(true);
        }
    }

    private IEnumerator RandomTeleport()
    {
        doingAction = true;

        // Play IDLE / TP Sounds
        sfxSource.PlayOneShot(SFX_Phase1Idle);
        finalBossAnimator.SetBool("isTeleporting", true);
        yield return new WaitForSeconds(0.5f); // Wait a sec for animation
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
       
        float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
        float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
        newPos = new Vector3(newX, finalBoss.transform.position.y, newZ);

        // Make sure new tp location is walkable distance away from old 
        while (Vector3.Distance(newPos, finalBoss.transform.position) < 5f)
        {
            Debug.Log("FINDING NEW SPOT");
            newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
            newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
            newPos = new Vector3(newX, finalBoss.transform.position.y, newZ);
        }


        finalBoss.transform.position = newPos;
        finalBossAnimator.SetBool("isTeleporting", false);

        // Reset actions
        actionsToTeleport = Random.Range(minTPActions, maxTPActions + 1);
        actionsDone = 0;

        yield return new WaitForSeconds(0.7f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
    }

    private IEnumerator SpawnShadowHands()
    {
        // Change speed
        if (state == BOSS_STATE.PHASE_TWO && !isMelee)
            huntSpeed = originalHuntSpeed + 0.5f;

        doingAction = true;

        // Play cast animation
        finalBossAnimator.SetBool("isCasting", true);
        yield return new WaitForSeconds(0.6f); // Wait for animation
        finalBossAnimator.SetBool("isCasting", false);

        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        if (state == BOSS_STATE.PHASE_ONE)
        {
            numberOfHands = 10;
        }
        else if (state == BOSS_STATE.PHASE_TWO)
        {
            numberOfHands = 14;
        }

        for (int i = 0; i < numberOfHands; i++)
        {
            float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
            float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
            newPos = new Vector3(newX, shadowHandPrefab.transform.position.y, newZ);

            GameObject shadowHand = Instantiate(shadowHandPrefab, finalBoss.transform);
            float randScale = Random.Range(1f, 3f);
            shadowHand.transform.localScale = new Vector3(randScale, randScale, 1);
            shadowHand.transform.parent = null;

            // Play Shadow Hands SFX
            sfxSource.PlayOneShot(SFX_ShadowHand);
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

        // Play cast animation
        finalBossAnimator.SetBool("isCasting", true);
        yield return new WaitForSeconds(0.5f); // Wait for animation
        finalBossAnimator.SetBool("isCasting", false);

        // Set projectile speed
        orbForce = 100;
        float gapDuration = Random.Range(0f, 0.1f);

        float randomAngle = 0f;

        // Play Cast SFX
        sfxSource.PlayOneShot(SFX_Phase1Cast);

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
        // Change speed
        if (state == BOSS_STATE.PHASE_TWO && !isMelee)
            huntSpeed = originalHuntSpeed - 0.5f;

        doingAction = true;

        // Set projectile speed
        orbForce = 300f;

        for (int i = 0; i < 5; i++)
        {
            // Play cast animation
            finalBossAnimator.SetBool("isCasting", true);
            yield return new WaitForSeconds(0.6f); // Wait for animation
            finalBossAnimator.SetBool("isCasting", false);

            // Play Cast SFX
            sfxSource.PlayOneShot(SFX_Phase1Cast);

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

        // Play cast animation
        finalBossAnimator.SetBool("isCasting", true);
        // Play channel animation
        finalBossAnimator.SetBool("isChanneling", true);

        // Play Channel SFX
        sfxSource.PlayOneShot(SFX_Phase1Channel);
        yield return new WaitForSeconds(4f); // Longer animation on purpose, accomodating SFX
        finalBossAnimator.SetBool("isChanneling", false);
        finalBossAnimator.SetBool("isCasting", false);

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
        // Play Cast SFX
        sfxSource.PlayOneShot(SFX_Phase1Cast);
        orbRB.AddForce(directionToPlayer * orbForce);


        yield return new WaitForSeconds(2f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = Random.Range(0, 4);
        actionsDone++;
    }

    public void DamageBoss()
    {
        if (state == BOSS_STATE.PHASE_ONE && !isInvincible)
        {
            // Play hit particles
            hitParticles.Play();

            currentHP -= 0.1f; // 10 hits to die

            // Insta TP First Boss on hit
            actionsDone = actionsToTeleport;
        }
        else if (state == BOSS_STATE.PHASE_TWO && !isInvincible)
        {
            // Play hit particles
            hitParticles.Play();

            currentHP -= 0.066f; // 15 hits to die
        }
        
        hpBar.fillAmount = currentHP;
    }

    private void TurnOffAllPhase2Animations()
    {
        finalBossAnimator.SetBool("isIdleToPuddle", false);
        finalBossAnimator.SetBool("isPuddle", false);
        finalBossAnimator.SetBool("isPuddleToIdle", false);
        finalBossAnimator.SetBool("isHunting", false);
        finalBossAnimator.SetBool("isMelee", false);
    }

    private void TurnOffAllPhase1Animations()
    {
        finalBossAnimator.SetBool("isChanneling", false);
        finalBossAnimator.SetBool("isTeleporting", false);
        finalBossAnimator.SetBool("isCasting", false);
    }

    public void SetIsRooted(bool flag)
    {
        isRooted = flag;
    }

    public IEnumerator RootBoss(float rootDuration)
    {  
        isRooted = true;
        finalBossSpriteRenderer.color = new Color(0.753f, 0.933f, 1f, 1f); // Light Blue color
        yield return new WaitForSeconds(rootDuration);
        finalBossSpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white
        isRooted = false;
    }
}
