using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathNode;
    [SerializeField] private float speed;
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
        MoveBoss();
    }

    public void MoveBoss()
    {
        //Debug.Log("Chosen Path " + chosenPath);
        if (transform.position != pathNode[chosenPath].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathNode[chosenPath].transform.position, speed * Time.deltaTime);
        }
        else // if boss reaches destination
        {
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
