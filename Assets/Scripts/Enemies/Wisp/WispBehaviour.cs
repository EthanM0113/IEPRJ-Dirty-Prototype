using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviour : BaseEnemy
{
    FaceDirection faceDirection;
    private float faceTicks = 0.0f;
    [SerializeField] private float FACE_INTERVAL = 1.0f;
    [SerializeField] private float minFaceInterval = 0.7f;
    [SerializeField] private float maxFaceInterval = 1.5f;
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
            Random.InitState(Random.Range(int.MinValue, int.MaxValue));
            FACE_INTERVAL = Random.Range(minFaceInterval, maxFaceInterval);
            faceTicks = 0.0f;  
        }
    }
}
