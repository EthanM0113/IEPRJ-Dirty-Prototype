using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaceDirection : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject detectionCone;

    NavMeshAgent agent;
    bool isFacingRight;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        int chooseFaceDirection = Random.Range(0, 2);
        if(chooseFaceDirection == 0)
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0,90,0);

        }
        else
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    private void Update()
    {
        if (agent.velocity.x > 0.1f && !isFacingRight) // if the agent is moving right but not facing right, then flip
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if(agent.velocity.x < -0.1f && isFacingRight)
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public bool GetFaceDirection()
    {
        return isFacingRight;
    }
}
