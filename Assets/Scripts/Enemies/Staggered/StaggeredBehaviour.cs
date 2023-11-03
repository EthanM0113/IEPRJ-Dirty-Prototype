using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredBehaviour : BaseEnemy
{
    Rigidbody rb;
    Animator anim;

    // how long the enemy waits after getting into a point. Two values to randomize the wait time
    [SerializeField] float startWaitTime = 2f; // minimum wait time
    [SerializeField] float endWaitTime = 4f; // maximum wait time

    [SerializeField] float AoE_radius = 3f; // radius of the staggereds effect
    [SerializeField] float slowDuration = 0.5f; // How long does the slow effect lasts
    [SerializeField] float slowSpeed = 60f; // Speed of the player when slowed
    [SerializeField] LayerMask playerLayer; 

    [SerializeField] float minFlipTime = 3f; // Minimum time it takes for the enemy to turn
    [SerializeField] float maxFlipTime = 6f; // Maximum time it takes for the enemy to turn
    float flipTimer;
    float waitTimer; // the actual timer/ cooldown
    FaceDirection faceRef;
    bool faceRight = false;
    // the node to traverse to. The node that the enemy needs to be at
    int currentNode = 2;

    [SerializeField] Transform[] route;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        faceRef = GetComponent<FaceDirection>();
    }

    public override void Activate() // Everytime the enemy is spawned
    {
        route = baseRouteNode.GetComponentsInChildren<Transform>(); // Gets the node List
        isActivated = true;
        currentState = State.STATIONARY;
        waitTimer = Random.Range(startWaitTime, endWaitTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActivated)
        {
            Move();
            UpdateRadius();
        }
    }

    private void Move()
    {
        if (flipTimer < Time.time)
        {
            faceRef.Flip(faceRight);
            faceRight = !faceRight;
            flipTimer = Time.time + Random.Range(minFlipTime, maxFlipTime);
        }


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
            if (Vector3.Distance(transform.position, route[currentNode].position) < 0.8) // if the transform is near the target node go to next node
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
            }
            else if (route[currentNode] == null)
            {
                currentNode = 1;
            }
            
            anim.SetBool("IsMoving", true);
        }
    }

    private void UpdateRadius()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, AoE_radius, playerLayer);

        if (col.Length == 0) return;

        col[0].gameObject.GetComponent<PlayerController>().SlowDebuff(slowDuration, slowSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AoE_radius);
    }

    public void MoveFromAnimation() 
    {
        // gets the direction to move to
        Vector3 direction = new Vector3(
            route[currentNode].position.x - transform.position.x,
            0,
            route[currentNode].position.z - transform.position.z
            );
        // Moves the rigidbody
        rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }

    public override void EnemyDeath()
    {
        anim.SetTrigger("Dead");
        base.EnemyDeath();
    }
}
