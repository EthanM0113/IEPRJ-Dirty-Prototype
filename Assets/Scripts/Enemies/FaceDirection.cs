using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaceDirection : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject detectionCone;
    [SerializeField] bool canMove = true;

    //NavMeshAgent agent;
    Rigidbody rb;
    bool isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if (canMove) {
            if (rb.velocity.x > 0.1f && !isFacingRight) // if the agent is moving right but not facing right, then flip
            {
                isFacingRight = true;
                sprite.flipX = !isFacingRight;
                detectionCone.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (rb.velocity.x < -0.1f && isFacingRight)
            {
                isFacingRight = false;
                sprite.flipX = !isFacingRight;
                detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }

    public bool GetFaceDirection()
    {
        return isFacingRight;
    }

    public void Flip(bool isRight)
    {
        if (!isRight)
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, 90, 0);

        }
        else
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public void FlipSprite(bool isRight)
    {
        if (!isRight)
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
        }
        else
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isFacingRight)
            {
                isFacingRight = true;
                sprite.flipX = !isFacingRight;
                detectionCone.transform.rotation = Quaternion.Euler(0, 90, 0);

            }
            else
            {
                isFacingRight = false;
                sprite.flipX = !isFacingRight;
                detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }
}
