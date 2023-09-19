using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathNode;
    [SerializeField] private float speed;
    [SerializeField] private FirstBossUIManager firstBossUIManager;
    private int pathNodeCount;
    private int chosenPath;

    // Start is called before the first frame update
    void Start()
    {
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
        //Debug.Log("Chosen Path " + chosenPath);
        float distanceBetween = Vector3.Distance(transform.position, pathNode[chosenPath].transform.position);
        //Debug.Log("Distance between: " + distanceBetween);
        if (distanceBetween >= 0.5)
        {
            Debug.Log("MOVING");
            transform.position = Vector3.MoveTowards(transform.position, pathNode[chosenPath].transform.position, speed * Time.deltaTime);
        }
        else if (distanceBetween < 0.5) // if boss reaches destination
        {
            Debug.Log("REACHED!");
            ChooseRandomPath();
        }
    }

    public void ChooseRandomPath()
    {
        chosenPath = Random.Range(0, pathNodeCount);
    }

    public void IncreaseSpeed(float num)
    {
        speed += num;
    }

    public void SetSpeed(float num)
    {
        speed = num;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
