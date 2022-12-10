using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] GameObject deadVersion;
    protected Transform baseRouteNode;

    public virtual void Activate()
    {

    }

    // Spawns a dead version of the 
    public virtual void EnemyDeath()
    {
        Instantiate(deadVersion, transform.position, Quaternion.identity);
    }

    public virtual void SetRouteNode(Transform node)
    {
        baseRouteNode = node;
    }
}
