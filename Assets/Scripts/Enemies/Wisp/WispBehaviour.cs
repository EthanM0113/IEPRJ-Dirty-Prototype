using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviour : BaseEnemy
{
    FaceDirection faceDirection;
    private float faceTicks = 0.0f;
    [SerializeField] private float FACE_INTERVAL = 1.0f;
    bool faceRight = true;
    [SerializeField] private Animator wispAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        faceDirection = GetComponent<FaceDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        faceTicks += Time.deltaTime;
        if (faceTicks >= FACE_INTERVAL)
        {
            wispAnimator.SetBool("didTP", true);
            faceDirection.Flip(!faceRight);
            faceRight = !faceRight;
            faceTicks = 0.0f;
        }
        else
        {
            wispAnimator.SetBool("didTP", false);
        }
    }
}
