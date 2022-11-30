using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] float coneAngle = 90f;
    [SerializeField] Transform lookPoint;
    [SerializeField] float detectionRange = 8f;

    [SerializeField] Light cone;
    [SerializeField] Color undetectedColor;
    [SerializeField] Color detectedColor;

    Transform playerTransform;

    bool isPlayerDetected = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        cone.color = undetectedColor;
        isPlayerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = playerTransform.position - transform.position;
        float angle = Vector3.Angle(dir, lookPoint.forward);
        RaycastHit r;

        if (angle < coneAngle / 2)
        {

            if (Physics.Raycast(lookPoint.position, dir, out r, detectionRange))
            {
                if (r.collider.gameObject.CompareTag("Player")) {

                    Debug.DrawRay(lookPoint.position, dir, Color.red);
                    cone.color = detectedColor;
                    isPlayerDetected = true;
                }
                else
                {
                    Debug.DrawRay(lookPoint.position, dir, Color.green);
                    cone.color = undetectedColor;
                    isPlayerDetected = false;
                }
                
            }
        }
    }
    public bool IsPlayerDetected()
    {
        return isPlayerDetected;
    }
}
