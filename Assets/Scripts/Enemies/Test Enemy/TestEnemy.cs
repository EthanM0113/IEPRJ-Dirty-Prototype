using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : BaseEnemy
{
    NavMeshAgent agent;

    // Skips the parent node
    int currentNode = 1;

    [SerializeField] Transform[] route;

    bool isActivated = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public override void Activate() // Everytime the enemy is spawned
    {
        route = baseRouteNode.GetComponentsInChildren<Transform>(); // Gets the node List
        isActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            Move();
        }
    }

    private void Move()
    {
        if (this.transform.position.x == route[currentNode].position.x &&
            this.transform.position.z == route[currentNode].position.z) // if the agent is on the node of the path
        {
            if (currentNode == route.Length - 1)// checks if the agent has reached the endpoint
            {
                currentNode = 1;
            }
            else if (currentNode < route.Length) // going towards the goal
            {
                currentNode++;

            }
        }
        agent.destination = route[currentNode].position; // moves the agent
    }
}
