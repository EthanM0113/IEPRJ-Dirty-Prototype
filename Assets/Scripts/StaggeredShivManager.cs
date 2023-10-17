using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredShivManager : MonoBehaviour
{
    private Rigidbody enemyRB;
    private SpriteRenderer enemySpriteRenderer;
    private WispBehaviour enemyWispBehavior;
    [SerializeField] private float rootDuration;

    private bool didRoot = false;
    private bool canRoot = true; // should only be able to root once
    private bool isWisp = false;

    private bool waitingForRoot = false;

    // Start is called before the first frame update
    void Start()
    {

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

                Debug.Log("Triggering Root Wisp.");
                //mainCameraAnimator.SetTrigger("isQuickZoom");
                SoundManager.Instance.StaggeredShiv();

                // Get contacted enemy's rigidbody
                enemyRB = other.gameObject.GetComponent<Rigidbody>();
                didRoot = true;

            }
            else // Check if other enemy w/ path based movement
            {
                enemySpriteRenderer = other.GetComponentInChildren<SpriteRenderer>();

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

        enemySpriteRenderer.color = new Color(1f, 0.561f, 0f, 1f); // Brown color
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

        enemySpriteRenderer.color = new Color(1f, 0.561f, 0f, 1f); // Brown color
        //enemyRB.constraints = RigidbodyConstraints.FreezeAll;
        enemyWispBehavior.SetIsRooted(true);
        yield return new WaitForSeconds(rootDuration);
        enemyWispBehavior.SetIsRooted(false);
        enemySpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white

        didRoot = false;
        gameObject.SetActive(false);
    }

}
