using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathNode;
    [SerializeField] private float speed;
    private float modifiableSpeed;
    private bool isRooted = false;
    [SerializeField] private FirstBossUIManager firstBossUIManager;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SpriteRenderer spriteRenderer; 
    private int pathNodeCount;
    private int chosenPath;
    private bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        modifiableSpeed = speed;
        pathNodeCount = pathNode.Count;
        ChooseRandomPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(firstBossUIManager.GetPlayerWithinRange())
        MoveBoss();
    }

    public void MoveBoss()
    {
        if(isMoving)
        {
            //Debug.Log("Chosen Path " + chosenPath);
            float distanceBetween = Vector3.Distance(transform.position, pathNode[chosenPath].transform.position);
            //Debug.Log("Distance between: " + distanceBetween);
            if (distanceBetween >= 0.5)
            {
                Debug.Log("MOVING");
                speed = modifiableSpeed;
                if (isRooted)
                    speed = 0f;
                transform.position = Vector3.MoveTowards(transform.position, pathNode[chosenPath].transform.position, speed * Time.deltaTime);
            }
            else if (distanceBetween < 0.5) // if boss reaches destination
            {
                Debug.Log("REACHED!");
                ChooseRandomPath();
            }
        }
        
    }

    public void ChooseRandomPath()
    {
        chosenPath = Random.Range(0, pathNodeCount);
    }

    public void IncreaseSpeed(float num)
    {
        modifiableSpeed += num;
    }

    public void SetSpeed(float num)
    {
        modifiableSpeed = num;
    }

    public float GetSpeed()
    {
        return modifiableSpeed;
    }

    public void DisableMovement()
    {
        isMoving = false;
    }

    public void PlayParticle(Vector3 location)
    {
        particle.transform.position = location;
        particle.Play();
    }

    public IEnumerator RootMovement(float rootDuration)
    {
        spriteRenderer.color = new Color(0.753f, 0.933f, 1f, 1f); // Light Blue color
        isRooted = true;
        yield return new WaitForSeconds(rootDuration);
        isRooted = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // back to white
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;  
    }
}
