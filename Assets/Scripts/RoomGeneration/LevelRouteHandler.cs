using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRouteHandler : MonoBehaviour
{
    // List of parent routes
    [SerializeField] List<GameObject> RouteList;
    private List<List<GameObject>> RouteNodeList;

    // Object pooler
    private GameObject objectPooler;
    private SamplePooler samplePooler;
        
    // Start is called before the first frame update
    void Start()
    {
        // Initialize
        List<List<GameObject>> RouteNodeList = new List<List<GameObject>>();

        Debug.Log("Happens Once!");
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler");
        samplePooler = objectPooler.GetComponent<SamplePooler>();
        samplePooler.PoolerTest();

        // Pass Route List Parents 1D Array
        samplePooler.PassRouteList(RouteList);

        // Pass Route Node List, 2D Array, 1st Element is Spawn Node
        ParseParentList(RouteList, RouteNodeList);
        samplePooler.PassRouteNodeList(RouteNodeList);
        samplePooler.PrintRouteNodeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseParentList(List<GameObject> parentList, List<List<GameObject>> childrenList) 
    {
        for(int i = 0; i < parentList.Count; i++)
        {
            List<GameObject> tempChildList = new List<GameObject>();
            tempChildList = GetAllChildren(parentList[i]);
            childrenList.Add(tempChildList);
        }
    }

    public List<GameObject> GetAllChildren(GameObject parent)
    {
        List<GameObject> childList = new List<GameObject>();
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            childList.Add(parent.transform.GetChild(i).gameObject);
            Debug.Log("Added" + i);
        }


        return childList;
    }
}
