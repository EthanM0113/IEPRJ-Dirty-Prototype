using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEnemy : BaseEnemy
{
    // Used to know the rotation direction
    enum Rotation{
        Clockwise = 0, CounterClockwise
    }

    [SerializeField] float rotationPerInterval = 45f; // How much should sentry rotate per tick
    float currentRotation = 0f;
    [SerializeField] float interval = 1f; // Every hom many seconds should the sentry rotate

    [SerializeField] Rotation direction = Rotation.Clockwise; // Rotation Direction
    float dirMultiplier = 1f; // Used for computing the direction

    [SerializeField] Transform lightSource; // Reference to the light source holder

    FaceDirection faceDir;

    float intervalTime = 0f;

    bool facingRight = true;

    // Start is called before the first frame update
    void Awake()
    {
        faceDir= GetComponent<FaceDirection>();

        currentState = State.STATIONARY;
        intervalTime = Time.time + interval;
        if (direction == Rotation.CounterClockwise)
        {
            dirMultiplier = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (intervalTime <= Time.time)
        {
            lightSource.RotateAround(lightSource.position, Vector3.up, rotationPerInterval * dirMultiplier);
            intervalTime = Time.time + interval;

            currentRotation += rotationPerInterval;
            if (currentRotation >= 360f || currentRotation <= -0.01f)
            {
                currentRotation = 0;
            }

            if (Mathf.Abs(currentRotation) == 90f || Mathf.Abs(currentRotation) == 270f)
            {
                facingRight = !facingRight;

                faceDir.FlipSprite(facingRight); // flip face direction // edit to not change sprite
            }
            Debug.Log(currentRotation);

        }
    }
}
