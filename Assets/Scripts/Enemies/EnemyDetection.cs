using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.ProBuilder.Shapes;

public class EnemyDetection : MonoBehaviour
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
    [SerializeField] bool isGargoyle = false;
    [SerializeField] LayerMask playerLayer;
    PlayerHearts playerHealth;
    FuelBarHandler fuelBarHandler;
    Animator anim;
    ObjectPooler pooler;
    RoomConditions room;

    bool isPlayerDetected = false; // Checks if the player is detected
    EnemySpawner spawnerRef = null; // gets the reference of the spawner it came from

    GameObject respawnNode;

    FaceDirection faceDirection;
    [SerializeField] bool rotateOnCollision = true;

    private PlayerController playerController;
    private Animator playerAnimator;
    private PlayerInputHandler playerInputHandler;  

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHearts>();
        pooler = ObjectPooler.Instance;
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
        if(playerController.GetIsPlayerDetectable())
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
                    playerController.SetPreventMovementInput(true);
                    if(!isGargoyle)
                        anim.SetTrigger("ATK");
                    else
                        playerHealth.DamagePlayer(1);
                    playerInputHandler.SetCanInput(true);
                    playerController.SetPreventMovementInput(false);
                    spawnerRef = null;
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
    public bool IsPlayerDetected()
    {
        return isPlayerDetected;
    }

    public void SetSpawnerReference(EnemySpawner spawnerRef)
    {
        this.spawnerRef = spawnerRef;
    }

    private void OnCollisionStay(Collision collision)
    {

        if (room = collision.gameObject.GetComponentInParent<RoomConditions>())
        {
            room = collision.gameObject.GetComponentInParent<RoomConditions>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rotateOnCollision) return;

        if(startDetectionTimer)
            faceDirection.RotateOnCollision(collision);
    }

    public void DamagePlayer()
    {
        playerHealth.DamagePlayer(1);
    }

}

