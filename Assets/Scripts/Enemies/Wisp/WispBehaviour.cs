using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviour : BaseEnemy
{
    FaceDirection faceDirection;
    private float faceTicks = 0.0f;
    [SerializeField] private float FACE_INTERVAL = 1.0f;
    bool faceRight = true;

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
            faceDirection.Flip(!faceRight);
            faceRight = !faceRight;
            faceTicks = 0.0f;
        }
    }
}
