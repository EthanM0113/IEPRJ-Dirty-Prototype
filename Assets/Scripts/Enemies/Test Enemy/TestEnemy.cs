using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;

public class TestEnemy : BaseEnemy
{
    Rigidbody rb;
    Animator anim;

    // how long the enemy waits after getting into a point. Two values to randomize the wait time
    [SerializeField] float startWaitTime = 2f; // minimum wait time
    [SerializeField] float endWaitTime = 4f; // maximum wait time
    float waitTimer; // the actual timer/ cooldown

    // the node to traverse to. The node that the enemy needs to be at
    int currentNode = 2;

    [SerializeField] Transform[] route;

    AudioSource sfxSource;
    [SerializeField] AudioClip perishSFX;
    [SerializeField] AudioClip idleSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sfxSource = GetComponent<AudioSource>();
    }

    public override void Activate() // Everytime the enemy is spawned
    {
        route = baseRouteNode.GetComponentsInChildren<Transform>(); // Gets the node List
        isActivated = true;
        currentState = State.STATIONARY;
        waitTimer = Random.Range(startWaitTime, endWaitTime);
        //sfxSource.clip = idleSFX;
        //sfxSource.volume = 0.8f * SoundManager.Instance.GetSFXMultiplier();
        //sfxSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActivated)
        {
            Move();
            sfxSource.volume = 0.8f * SoundManager.Instance.GetSFXMultiplier();
            //Debug.Log("Volume " + SoundManager.Instance.GetSFXMultiplier());
        }

    }

    private void Move()
    {
        
        if (currentState == State.STATIONARY) // If the enemy is stationary
        {
            if (waitTimer <= 0)
            { // When timer ends
                waitTimer = Random.Range(startWaitTime, endWaitTime);
                currentState = State.MOVING;
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
            anim.SetBool("IsMoving", false);
        }
        else if (currentState == State.MOVING)
        {
            if (Vector3.Distance(transform.position, route[currentNode].position) < 0.2) // if the transform is near the target node go to next node
            {
                currentState = State.STATIONARY;
                if (currentNode == route.Length - 1)// checks if the agent has reached the end node
                {
                    currentNode = 1;
                }
                else if (currentNode < route.Length) // going towards the next node
                {
                    currentNode++;
                }
            }else if (route[currentNode] == null)
            {
                currentNode = 1;
            }

            // gets the direction to move to
            Vector3 direction = new Vector3(
                route[currentNode].position.x - transform.position.x,
                0,
                route[currentNode].position.z - transform.position.z
                );

            // Moves the rigidbody
            rb.velocity = new Vector3
                        (
                           direction.normalized.x * speed * Time.deltaTime,
                           rb.velocity.y,
                           direction.normalized.z * speed * Time.deltaTime
                        );
            anim.SetBool("IsMoving", true);
        }
    }

    public override void EnemyDeath()
    {
        SoundManager.Instance.EnemyPerish(perishSFX);
        base.EnemyDeath();
    }
    public override void PauseEnemy()
    {
        sfxSource.mute = true;
        isActivated = false;
    }

    public override void ResumeEnemy()
    {
        sfxSource.mute = false;
        isActivated = true;
    }
}
