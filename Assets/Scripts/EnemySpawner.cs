using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    ObjectPooler objPooler;
  
   
    [SerializeField] GameObject[] NodeList;
    [SerializeField] float SpawnInterval = 1.5f;
    private float Timer;
    void Start()
    {
        objPooler = ObjectPooler.Instance;
        Timer = SpawnInterval;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Timer < 0f)
        {
            //int Enemy = Random.Range(1, 4); //1 is Green, 2 is Blue, 3 is Red
            int Enemy = 1;
            int Node;
            switch (Enemy)
            {
                case 1:
                    Node = Random.Range(1, 4);
                    objPooler.SpawnFromPool("TestEnemy", NodeList[Node - 1].transform.position);
                    break;
                //case 2:
                //    Node = Random.Range((NodeList.Length / 3) + 1, (NodeList.Length / 3) * 2 + 1);
                //    objPooler.SpawnFromPool("BlueEnemy", NodeList[Node - 1].transform.position);
                //    break;
                //case 3:
                //    Node = Random.Range((NodeList.Length / 3) * 2 + 1, (NodeList.Length / 3) * 3 + 1);
                //    objPooler.SpawnFromPool("RedEnemy", NodeList[Node - 1].transform.position);
                //    break;
            }
                Timer = SpawnInterval;
        }
        else
        {
            Timer -= Time.deltaTime;
        }
    }

    
}
