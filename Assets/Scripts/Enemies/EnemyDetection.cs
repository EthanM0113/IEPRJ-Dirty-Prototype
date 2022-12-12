using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] float coneAngle = 90f;
    [SerializeField] Transform lookPoint;
    [SerializeField] float detectionRange = 8f;

    [SerializeField] Light cone;
    [SerializeField] Color undetectedColor;
    [SerializeField] Color detectedColor;

    Transform playerTransform;
    PlayerHearts playerHealth;
    Animator anim;
    ObjectPooler pooler;

    bool isPlayerDetected = false; // Checks if the player is detected
    EnemySpawner spawnerRef = null; // gets the reference of the spawner it came from


    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHearts>();
        pooler = ObjectPooler.Instance;
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
                    //playerHealth.DamagePlayer(1);
                    pooler.DisableAll();
                    spawnerRef.SpawnAll();
                    spawnerRef = null;

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

    public void SetSpawnerReference(EnemySpawner spawnerRef)
    {
        this.spawnerRef = spawnerRef;
    }
}

