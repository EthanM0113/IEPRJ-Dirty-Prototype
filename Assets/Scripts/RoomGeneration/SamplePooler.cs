using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePooler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> RouteList;
    public List<List<GameObject>> RouteNodeList;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void PoolerTest()
    {
        Debug.Log("Pooler Found!");
    }

    public void PassRouteList(List<GameObject> routeList)
    {
        RouteList = routeList;
    }

    public void PassRouteNodeList(List<List<GameObject>> routeNodeList)
    {
        RouteNodeList = routeNodeList;
    }

    public void PrintRouteNodeList()
    {
        for(int i = 0; i < RouteNodeList.Count; i++)
        {
            for(int j = 0; j < RouteNodeList[i].Count; j++)
            {
                Debug.Log("Route " + i + ": " + RouteNodeList[i][j].name);
            }
        }
    }
}
