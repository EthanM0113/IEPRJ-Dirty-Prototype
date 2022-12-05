using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] GameObject deadVersion;

    public virtual void EnemyDeath()
    {
        Instantiate(deadVersion, transform.position, Quaternion.identity);
    }
}
