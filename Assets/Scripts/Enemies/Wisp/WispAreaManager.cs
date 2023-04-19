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
    private bool didSpawn = false;
    private float ticks = 0.0f;
    private GameObject assignedWisp;
    [SerializeField] private float TP_INTERVAL = 0.1f;
    public Animator wispAnimator;
    public WispAnimationManager wispAnimationManager;

    // Start is called before the first frame update
    void Start()
    {
        tlBoundsPos = TLBounds.gameObject.transform.position;
        brBoundsPos = BRBounds.gameObject.transform.position;
        if(wispAnimator != null)
        {
            Debug.Log("FOUND ANIMATOR " + wispAnimator.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Top Left: " + tlBoundsPos);
        //Debug.Log("BottomRight: " + brBoundsPos);
        ticks += Time.deltaTime;

        if (ticks >= TP_INTERVAL)
        {
            Random.InitState(Random.Range(int.MinValue, int.MaxValue));
            float newX = Random.Range(tlBoundsPos.x, brBoundsPos.x);
            float newZ = Random.Range(tlBoundsPos.z, brBoundsPos.z);
            Vector3 newPos = new Vector3(newX, tlBoundsPos.y, newZ);
            
            if (!didSpawn)
            {
                assignedWisp = GameObject.Instantiate(wispPrefab, newPos, wispPrefab.transform.rotation);
                didSpawn = true;
                wispAnimator = assignedWisp.GetComponentInChildren<Animator>();
                wispAnimationManager = assignedWisp.GetComponentInChildren<WispAnimationManager>();
                wispAnimator.SetBool("didTP", true);
            }
            else
            {
                wispAnimator.SetBool("didTP", true);
                assignedWisp.transform.position = newPos;
            }
            ticks = 0.0f;
        }
        else
        {
            if(didSpawn && wispAnimationManager.GetFinishedTP())
            {
                wispAnimator.SetBool("didTP", false);
            }
        }




    }

    public void SetAssignedWisp(GameObject wispToAssign)
    {
        assignedWisp = wispToAssign;
    }
}
