using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    [SerializeField] GameObject obj;
    private int enemyNumber = 0;
    private int enemyDead = 0;
    private bool isStart = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("TestEnemy"))
        {
            enemyNumber++;
            Debug.Log("Added Test");
        }

        if (collision.collider.CompareTag("DeadTestEnemy"))
        {
            enemyDead++;
            Debug.Log("Added Dead");
        }
    }

    public int GetTotalEnemy()
    {
        return enemyNumber; 
    }

    private void Update()
    {
            obj = this.transform.parent.gameObject;
        if (enemyDead == enemyNumber && isStart)
        {
            Debug.Log("They ded");
        }
    }

    public void setStart()
    {
        isStart = true;
    }
}
