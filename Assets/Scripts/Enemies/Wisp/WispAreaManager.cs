using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispAreaManager : MonoBehaviour
{
    [SerializeField] private GameObject wispPrefab;
    [SerializeField] private GameObject TLBounds;
    [SerializeField] private GameObject BRBounds;
    private Vector3 tlBoundsPos;
    private Vector3 brBoundsPos;
    private Vector3 newPos;
    private bool isTPing = false;
    private bool firstSpawn = true; // bool just to handle first time spawning
    private float ticks = 0.0f;
    private GameObject assignedWisp;
    [SerializeField] private float TP_INTERVAL = 0.1f;
    [SerializeField] private Animator wispAnimator;
    [SerializeField] private WispAnimationManager wispAnimationManager;
    [SerializeField] GameObject EnemyContainer;
    private Light wispSelfLight;
    private AudioSource sfxSource;
    private float sfxVolume = 0.1f;
    [SerializeField] private float defautlSFXVolume;

    // Start is called before the first frame update
    void Start()
    {
        tlBoundsPos = TLBounds.gameObject.transform.position;
        brBoundsPos = BRBounds.gameObject.transform.position;
        if(wispAnimator != null)
        {
            Debug.Log("FOUND ANIMATOR " + wispAnimator.name);
        }
        sfxSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly update audio
        if(assignedWisp != null)
        {
            if(!assignedWisp.GetComponent<WispBehaviour>().GetIsMute())
            {
                sfxVolume = defautlSFXVolume;
            }
            else
            {
                sfxVolume = 0f;
            }
        }

        // First time spawning
        if (firstSpawn)
        {
            Random.InitState(Random.Range(int.MinValue, int.MaxValue));
            float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
            float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
            Vector3 newPos = new Vector3(newX, tlBoundsPos.y, newZ);

            assignedWisp = GameObject.Instantiate(wispPrefab, newPos, wispPrefab.transform.rotation, EnemyContainer.transform);
            wispAnimator = assignedWisp.GetComponentInChildren<Animator>();
            wispAnimationManager = assignedWisp.GetComponentInChildren<WispAnimationManager>();
            assignedWisp.GetComponent<WispEnemyDetection>().SetAreaManager(this);
            wispSelfLight = assignedWisp.GetComponent<WispBehaviour>().GetSelfLight();
            firstSpawn = false;
        }
        else
        {
            if(!assignedWisp.GetComponent<WispBehaviour>().GetIsRooted())
            {
                // Increment only after spawning for the first time
                if (assignedWisp.GetComponent<WispBehaviour>().GetBehaviour())
                {
                    ticks += Time.deltaTime;

                }
            }         
        }

        if(!assignedWisp.GetComponent<WispBehaviour>().GetIsRooted())
        {
            if (ticks >= TP_INTERVAL)
            {
                if (!isTPing)
                {
                    Random.InitState(Random.Range(int.MinValue, int.MaxValue));
                    float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
                    float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
                    newPos = new Vector3(newX, tlBoundsPos.y, newZ);
                    isTPing = true;
                    wispAnimator.SetBool("didTP", true);
                    wispAnimationManager.NotFinishedTP();
                    assignedWisp.GetComponent<WispBehaviour>().PlayPreTPParticles();
                    Debug.Log("Teleporting!");  
                    wispSelfLight.intensity = 140.0f;
                    sfxSource.volume = sfxVolume * SoundManager.Instance.GetSFXMultiplier();
                    sfxSource.Play();
                }

                if (wispAnimationManager.GetFinishedTP())
                {
                    wispSelfLight.intensity = 1.0f;
                    Debug.Log("Finished Teleport!");
                    isTPing = false;
                    wispAnimator.SetBool("didTP", false);
                    wispAnimationManager.FinishedTP();
                    assignedWisp.transform.position = newPos;
                    wispPrefab.GetComponent<WispBehaviour>().PlayTpParticles(newPos);
                    ticks = 0.0f;
                }
            }
        }  
    }

    public bool GetIsTeleporting()
    {
        return isTPing;
    }

    public void SetAssignedWisp(GameObject wispToAssign)
    {
        assignedWisp = wispToAssign;
    }
}
