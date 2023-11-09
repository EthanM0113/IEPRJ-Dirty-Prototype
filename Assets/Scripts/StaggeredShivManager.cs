using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredShivManager : MonoBehaviour
{
    private Rigidbody enemyRB;
    private SpriteRenderer enemySpriteRenderer;
    private WispBehaviour enemyWispBehavior;
    private SentryEnemy enemySentryBehavior;
    [SerializeField] private float rootDuration;
    [SerializeField] private GameObject rootOverlay;
    private GameObject srObject;
    private GameObject rootObject;
    private PlayerController playerController;  

    private bool didRoot = false;
    private bool canRoot = true; // should only be able to root once
    private bool isWisp = false;
    private bool isGargoyle = false;
    private bool didSpawnRootOverlay = false;

    private bool waitingForRoot = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if(playerController.GetAbilityLevel() != 1)
            rootDuration += (0.5f * playerController.GetAbilityLevel());
    }

    // Update is called once per frame
    void Update()
    {
        if(didRoot)
        {
            if (isWisp)
            {
                StartCoroutine(TriggerWispRootEffect());
            }
            else if(isGargoyle)
            {
                StartCoroutine(TriggerGargoyleRootEffect());
            }
            else 
            {
                StartCoroutine(TriggerRootEffect());        
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        // Delete Shiv on first wall contacted, might cause problems in rooms w/ no walls
        if(other.gameObject.name.Contains("Wall"))
        {
            Debug.Log("Wall Found: " + other.gameObject.name);  
            if (!didRoot)
                gameObject.SetActive(false);
        }

        if (other.CompareTag("TestEnemy") && canRoot)
        {
            canRoot = false;
         
            if (other.GetComponent<WispBehaviour>() != null) // Check if wisp
            {
                isWisp = true;

                enemyWispBehavior = other.GetComponent<WispBehaviour>();    
                enemySpriteRenderer = other.GetComponentInChildren<SpriteRenderer>();
                srObject = enemySpriteRenderer.gameObject;

                Debug.Log("Triggering Root Wisp.");
                //mainCameraAnimator.SetTrigger("isQuickZoom");
                SoundManager.Instance.StaggeredShiv();

                // Get contacted enemy's rigidbody
                enemyRB = other.gameObject.GetComponent<Rigidbody>();
                didRoot = true;

            }
            else if (other.GetComponent<SentryEnemy>() != null) // Check if Gargoyle
            {
                isGargoyle = true;

                enemySentryBehavior = other.GetComponent<SentryEnemy>();
                enemySpriteRenderer = other.GetComponentInChildren<SpriteRenderer>();
                srObject = enemySpriteRenderer.gameObject;

                Debug.Log("Triggering Root Gargoyle.");
                SoundManager.Instance.StaggeredShiv();

                // Get contacted enemy's rigidbody
                enemyRB = other.gameObject.GetComponent<Rigidbody>();
                didRoot = true;

            }
            else // Check if other enemy w/ path based movement
            {
                enemySpriteRenderer = other.GetComponentInChildren<SpriteRenderer>();
                srObject = enemySpriteRenderer.gameObject;

                Debug.Log("Triggering Root Enemy.");
                //mainCameraAnimator.SetTrigger("isQuickZoom");
                SoundManager.Instance.StaggeredShiv();

                // Get contacted enemy's rigidbody
                enemyRB = other.gameObject.GetComponent<Rigidbody>();
                didRoot = true;
            }           
        }

       
    }

    public IEnumerator TriggerRootEffect()
    {
        if(!didSpawnRootOverlay)
        {
            rootObject = Instantiate(rootOverlay, srObject.transform); // spawn root overlay on Sprite Renderer Game Object
            Destroy(rootObject, rootDuration);
            didSpawnRootOverlay = true;
        }
 
        enemySpriteRenderer.color = new Color(0.753f, 0.933f, 1f, 1f); // Light Blue color
        enemyRB.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(rootDuration);
        enemyRB.constraints = RigidbodyConstraints.None;
        enemyRB.constraints = RigidbodyConstraints.FreezeRotation;
        enemySpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white

        didRoot = false;

        gameObject.SetActive(false);
    }

    public IEnumerator TriggerWispRootEffect()
    {
        if(!didSpawnRootOverlay)
        {
            rootObject = Instantiate(rootOverlay, srObject.transform); // spawn root overlay on Sprite Renderer Game Object
            Destroy(rootObject, rootDuration);
            didSpawnRootOverlay = true;
        }

        enemySpriteRenderer.color = new Color(0.753f, 0.933f, 1f, 1f); // Light Blue color
        //enemyRB.constraints = RigidbodyConstraints.FreezeAll;
        enemyWispBehavior.SetIsRooted(false);
        yield return new WaitForSeconds(rootDuration);
        enemyWispBehavior.SetIsRooted(true);
        enemySpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white

        didRoot = false;

        gameObject.SetActive(false);
    }

    public IEnumerator TriggerGargoyleRootEffect()
    {
        if (!didSpawnRootOverlay)
        {
            rootObject = Instantiate(rootOverlay, srObject.transform); // spawn root overlay on Sprite Renderer Game Object
            Destroy(rootObject, rootDuration);
            didSpawnRootOverlay = true;
        }

        enemySpriteRenderer.color = new Color(0.753f, 0.933f, 1f, 1f); // Light Blue color
        //enemyRB.constraints = RigidbodyConstraints.FreezeAll;
        enemySentryBehavior.SetIsAlive(false);
        yield return new WaitForSeconds(rootDuration);
        enemySentryBehavior.SetIsAlive(true);
        enemySpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white

        didRoot = false;

        gameObject.SetActive(false);
    }


}
