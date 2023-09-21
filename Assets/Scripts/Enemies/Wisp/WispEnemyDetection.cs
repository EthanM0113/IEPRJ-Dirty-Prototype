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

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHearts>();
        //pooler = ObjectPooler.Instance;
        fuelBarHandler = FindObjectOfType<FuelBarHandler>();
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
                    // Find respawn node
                    respawnNode = GameObject.FindGameObjectWithTag("RespawnNode");
                    FindObjectOfType<PlayerAbilityHandler>().transform.position = respawnNode.transform.position;

                    //Sets the ability to NONE
                    //playerTransform.GetComponent<PlayerAbilityHandler>().SetCurrentAbility(Ability.Type.NONE);
                    //Reset KillCounter
                    //room.resetKills();
                    isPlayerDetected = false;

                    // Nerf player fuel and deal dmg
                    fuelBarHandler.ResetFuel(1.0f);
                    playerHealth.DamagePlayer(1);

                    //pooler.DisableAll();
                    //spawnerRef.SpawnAll();
                    //spawnerRef = null;
                    gameObject.SetActive(true);
                    cone.color = undetectedColor;

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
}
