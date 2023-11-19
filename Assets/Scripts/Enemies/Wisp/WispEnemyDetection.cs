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

    [SerializeField] float detectionTime = 0.7f; // testing
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
    private bool playOnce = false;
    [SerializeField] private ParticleSystem detectedVFX;

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

                            if (!playOnce)
                            {
                                SoundManager.Instance.PlayDetected();
                                detectedVFX.Play();
                                playOnce = true;
                            }

                            Debug.DrawRay(transform.position, dir, Color.red);

                        }
                        else
                        {
                            // not detected
                            cone.color = undetectedColor;
                            isPlayerDetected = false;
                            startDetectionTimer = false;
                            detectionTimer = detectionTime;
                            playOnce = false;

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
                    playOnce = false;
                }

                if (startDetectionTimer)
                {
                    if (detectionTimer <= 0f)
                    {
                        isPlayerDetected = false;
                        playerInputHandler.SetCanInput(false);
                        playerController.SetPreventMovementInput(true);
                        playerHealth.DamagePlayer(1, false);
                        playerInputHandler.SetCanInput(true);
                        playerController.SetPreventMovementInput(false);
                        //spawnerRef = null;
                        detectionTimer = detectionTime;
                        startDetectionTimer = false;
                    }
                    else
                    {
                        detectionTimer -= Time.deltaTime;
                        //Debug.Log(detectionTimer);
                    }
                }
            }
        }
        else
        {
            cone.enabled = false;
        }
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
