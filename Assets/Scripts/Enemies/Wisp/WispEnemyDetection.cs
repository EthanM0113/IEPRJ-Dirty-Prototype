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
        Vector3 dir = playerTransform.position - transform.position;
        float angle = Vector3.Angle(dir, lookPoint.forward);
        RaycastHit r;

        if (angle < coneAngle / 2)
        {

            if (Physics.Raycast(lookPoint.position, dir, out r, detectionRange))
            {
                if (r.collider.gameObject.CompareTag("Player"))
                {
                    Debug.DrawRay(lookPoint.position, dir, Color.red);
                    cone.color = detectedColor;
                    isPlayerDetected = true;
                    startDetectionTimer = true;
                }
                else
                {
                    Debug.DrawRay(lookPoint.position, dir, Color.green);
                    cone.color = undetectedColor;
                    isPlayerDetected = false;
                    detectionTimer = detectionTime;
                    startDetectionTimer = false;
                }

            }
        }

        if (startDetectionTimer)
        {
            if (detectionTimer <= 0f)
            {
                // Find respawn node
                respawnNode = GameObject.FindGameObjectWithTag("RespawnNode");
                playerTransform.position = respawnNode.transform.position;

                //Sets the ability to NONE
                //playerTransform.GetComponent<PlayerAbilityHandler>().SetCurrentAbility(Ability.Type.NONE);
                //Reset KillCounter
                room.resetKills();
                isPlayerDetected = false;

                // Nerf player fuel and deal dmg
                fuelBarHandler.resetFuel(1);
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
}
