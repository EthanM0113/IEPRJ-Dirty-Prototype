using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispEnemyDetection : MonoBehaviour
{
    [SerializeField] float coneAngle = 90f;
    [SerializeField] Transform lookPoint;
    [SerializeField] float detectionRange = 8f;

    [SerializeField] Light cone;
    [SerializeField] Color undetectedColor;
    [SerializeField] Color detectedColor;

    [SerializeField] float detectionTime = 1f;
    float detectionTimer;

    bool startDetectionTimer = false;

    [SerializeField] private WispAreaManager wispAreaManager;
    [SerializeField] LayerMask playerLayer;


    Transform playerTransform;
    PlayerHearts playerHealth;
    FuelBarHandler fuelBarHandler;
    Animator anim;
    //ObjectPooler pooler;
    RoomConditions room;

    bool isPlayerDetected = false; // Checks if the player is detected
    //EnemySpawner spawnerRef = null; // gets the reference of the spawner it came from

    GameObject respawnNode;

    FaceDirection faceDirection;
    [SerializeField] bool rotateOnCollision = true;

    private PlayerController playerController;
    private Animator playerAnimator;
    private PlayerInputHandler playerInputHandler;


    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHearts>();
        //pooler = ObjectPooler.Instance;
        fuelBarHandler = FindObjectOfType<FuelBarHandler>();
        faceDirection = GetComponent<FaceDirection>();
        playerController = FindObjectOfType<PlayerController>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();

    }

    private void OnEnable()
    {
        cone.color = undetectedColor;
        isPlayerDetected = false;
        detectionTimer = detectionTime;
        startDetectionTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!wispAreaManager.GetIsTeleporting())
        {
            cone.enabled = true;

            if (playerController.GetIsPlayerDetectable())
            {
                Collider[] objectsWithinRange = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
                if (objectsWithinRange.Length != 0)
                {
                    if (objectsWithinRange[0])
                    {
                        Vector3 dir = objectsWithinRange[0].transform.position - transform.position;
                        float angle = Vector3.Angle(dir, lookPoint.forward);
                        RaycastHit r;

                        if (angle < coneAngle / 2)
                        {
                            // Detected
                            cone.color = detectedColor;
                            isPlayerDetected = true;
                            startDetectionTimer = true;

                            Debug.DrawRay(transform.position, dir, Color.red);

                        }
                        else
                        {
                            // not detected
                            cone.color = undetectedColor;
                            isPlayerDetected = false;
                            startDetectionTimer = false;
                            detectionTimer = detectionTime;

                            Debug.DrawRay(transform.position, dir, Color.green);

                        }
                    }
                }
                else
                {
                    // not detected
                    cone.color = undetectedColor;
                    isPlayerDetected = false;
                    startDetectionTimer = false;
                    detectionTimer = detectionTime;
                }

                if (startDetectionTimer)
                {
                    if (detectionTimer <= 0f)
                    {
                        isPlayerDetected = false;
                        playerInputHandler.SetCanInput(false);
                        playerController.SetCanInput(false);
                        DamagePlayer();
                        playerInputHandler.SetCanInput(true);
                        playerController.SetCanInput(true);
                        //spawnerRef = null;
                        detectionTimer = detectionTime;
                        startDetectionTimer = false;
                    }
                }
            }
        }
        else
        {
            cone.enabled = false;
        }
    }

    private void DamagePlayer()
    {
        StartCoroutine(TriggerImpactFrame()); // Hollow Knight Damage Implementation

        // Nerf player fuel and deal dmg
        fuelBarHandler.ResetFuel(1.0f);
        playerHealth.DamagePlayer(1);
    }

    private IEnumerator TriggerImpactFrame()
    {
        playerAnimator.SetTrigger("Hit");
        yield return new WaitForSecondsRealtime(0.40f); // Fine tune to fill in the animation
        //Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        //playerController.PlayDeathParticles();
        //playerAnimator.SetTrigger("Idle");
        Time.timeScale = 1;
    }

    public bool IsPlayerDetected()
    {
        return isPlayerDetected;
    }

    public void SetSpawnerReference(EnemySpawner spawnerRef)
    {
        //this.spawnerRef = spawnerRef;
    }

    private void OnCollisionStay(Collision collision)
    {

        if (room = collision.gameObject.GetComponentInParent<RoomConditions>())
        {
            room = collision.gameObject.GetComponentInParent<RoomConditions>();
        }
    }

    public void SetAreaManager(WispAreaManager wam)
    {
        wispAreaManager = wam;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rotateOnCollision) return;

        if (startDetectionTimer)
            faceDirection.RotateOnCollision(collision);
    }
}
