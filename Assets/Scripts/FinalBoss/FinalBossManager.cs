using System;
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

    // General 
    [SerializeField] private bool doingAction;
    private float actionTicks = 0f;
    private float actionInterval = 1f;
    [SerializeField] private GameObject finalBoss;
    private int chosenAction = 1; // Start with medium projectile attack

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 1f;
        currentHP = maxHP;
        hpBar.fillAmount = currentHP;
        state = BOSS_STATE.PHASE_ONE;
        doingAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(state == BOSS_STATE.PHASE_ONE)
        {
            if(chosenAction == 0)
            {

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
            
        }

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
                Rigidbody orbRB = orb.GetComponent<Rigidbody>();
                // Just affecting x and y scale
                orb.transform.localScale = new Vector3(0.5f ,0.5f, 0.5f);

                #region Rotate Each Orb
                if (j == 0)
                {
                    Vector3 orbRotation = orb.transform.localEulerAngles;
                    orb.transform.localEulerAngles = orbRotation;
                    float randomAngle = UnityEngine.Random.Range(0f, 89f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 1)
                {
                    Vector3 orbRotation = orb.transform.localEulerAngles;
                    orb.transform.localEulerAngles = orbRotation;
                    float randomAngle = UnityEngine.Random.Range(90f, 179f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 2)
                {
                    Vector3 orbRotation = orb.transform.localEulerAngles;
                    orb.transform.localEulerAngles = orbRotation;
                    float randomAngle = UnityEngine.Random.Range(180f, 269f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                else if (j == 3)
                {
                    Vector3 orbRotation = orb.transform.localEulerAngles;
                    orb.transform.localEulerAngles = orbRotation;
                    float randomAngle = UnityEngine.Random.Range(270f, 359f);
                    Vector3 rightDirection = finalBoss.transform.right;
                    Vector3 newDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * rightDirection;
                    orbRB.AddForce(newDirection * orbForce);
                }
                #endregion
            }
            yield return new WaitForSeconds(0.4f); // Wait a sec before shooting
        }

        yield return new WaitForSeconds(2.0f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = UnityEngine.Random.Range(1, 3);
        
    }

    private IEnumerator ShootLargeProjectile()
    {
        doingAction = true;

        // Set projectile speed
        orbForce = 600f;

        GameObject orb = Instantiate(orbPrefab, finalBoss.transform);
        Rigidbody orbRB = orb.GetComponent<Rigidbody>();
        // Just affecting x and y scale
        orb.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        // Find Player Location
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer = directionToPlayer.normalized;
        orbRB.AddForce(directionToPlayer * orbForce);


        yield return new WaitForSeconds(2.0f); // Wait a sec before choosing new action
        doingAction = false;
        chosenAction = UnityEngine.Random.Range(1, 3);
    }

    public void DamageBoss(float dmg)
    {
        currentHP -= dmg;
        hpBar.fillAmount = currentHP;
    }


}
