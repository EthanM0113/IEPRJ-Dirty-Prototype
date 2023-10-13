using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    //ObjectPooler objPooler;
    EnemyTemplates templates;
    [SerializeField] List<GameObject> EnemyList;
    [SerializeField] string EnemyKey = "TestEnemy";
    
    // Parent Route Node
    [SerializeField] List<Transform> RouteList;
    [SerializeField] List<Transform> StationaryList;

    [SerializeField] GameObject EnemyContainer;

    [SerializeField] float SpawnInterval = 1.5f;
    private float Timer;

    bool hasHappenedOnce = false;

    void Start()
    {
        //objPooler = ObjectPooler.Instance;
        Timer = SpawnInterval;
        EnemyList = new List<GameObject>();
    }

    public void SpawnAll()
    {
        templates = FindObjectOfType<EnemyTemplates>();
        for (int i = 0; i < RouteList.Count; i++)
        {

            //GameObject newEnemy = objPooler.SpawnFromPool(EnemyKey, RouteList[i].GetComponentsInChildren<Transform>()[1].position); // Spawn at spawnPoint
            //BaseEnemy enemyRef = newEnemy.GetComponent<BaseEnemy>();
            //enemyRef.SetRouteNode(RouteList[i]); // Get the Parent Route Node
            //enemyRef.Activate(); // set needed path
            //enemyRef.GetComponent<EnemyDetection>().SetSpawnerReference(this);


            /* Creates a new enemy based on each route then adds them to a list*/
            GameObject newEnemy = Instantiate(templates.EnemyList[0], RouteList[i].GetComponentsInChildren<Transform>()[1].position, Quaternion.identity, EnemyContainer.transform);
            BaseEnemy enemyRef = newEnemy.GetComponent<BaseEnemy>();
            EnemyList.Add(newEnemy);
            enemyRef.SetRouteNode(RouteList[i]); // Get the Parent Route Node
            enemyRef.Activate(); // set needed path
            enemyRef.GetComponent<EnemyDetection>().SetSpawnerReference(this);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasHappenedOnce)
        {
            SpawnAll();
            hasHappenedOnce = true;
        }


        ////Timer
        //if (Timer < 0f)
        //{
        //    //int Enemy = Random.Range(1, 4); //1 is Green, 2 is Blue, 3 is Red
        //    int Enemy = 1;
        //    int Node = 1;
        //    switch (Enemy)
        //    {
        //        case 1:
        //            Node = Random.Range(0, RouteList.Count);
        //            GameObject newEnemy = objPooler.SpawnFromPool("TestEnemy", RouteList[Node].GetComponentsInChildren<Transform>()[1].position);
        //            BaseEnemy enemyRef = newEnemy.GetComponent<BaseEnemy>();
        //            enemyRef.SetRouteNode(RouteList[Node]);
        //            enemyRef.Activate();

        //            break;
        //            //case 2:
        //            //    Node = Random.Range((NodeList.Length / 3) + 1, (NodeList.Length / 3) * 2 + 1);
        //            //    objPooler.SpawnFromPool("BlueEnemy", NodeList[Node - 1].transform.position);
        //            //    break;
        //            //case 3:
        //            //    Node = Random.Range((NodeList.Length / 3) * 2 + 1, (NodeList.Length / 3) * 3 + 1);
        //            //    objPooler.SpawnFromPool("RedEnemy", NodeList[Node - 1].transform.position);
        //            //    break;
        //    }
        //    Timer = SpawnInterval;
        //}
        //else
        //{
        //    Timer -= Time.deltaTime;
        //}
    }

    // Used for the spawn points
    public void Push_Back_Node(Transform node)
    {
        RouteList.Add(node); // Parent Route Node
    }


}
