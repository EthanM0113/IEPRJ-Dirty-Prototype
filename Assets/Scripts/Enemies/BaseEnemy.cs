using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected enum State
    {
        STATIONARY = 0,
        MOVING
    }

    protected State currentState;

    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float deathTimeOffset = 0f;
    [SerializeField] GameObject deadVersion;
    [SerializeField] GameObject miniMapSprite;
    protected float showSpriteDuration = 8.0f;
    protected Transform baseRouteNode;
    // determines if the enemy is activated
    [SerializeField] protected bool isActivated = false;
    protected bool isAlive = true;
    private void Start()
    {
        miniMapSprite.SetActive(false);

    }
    public virtual void Activate() // when the enemy is spawned
    {

    }


    public void PauseEnemy()
    {
        isActivated = false;
    }

    public void ResumeEnemy()
    {
        isActivated = true;
    }
    public bool GetBehaviour()
    {
        return isActivated;
    }

    // Spawns a dead version of the enemy
    public virtual void EnemyDeath()
    {
        if (deadVersion != null)
        {
            Invoke("SpawnDeadVersion", deathTimeOffset);
        }
        else
        {
            Debug.Log($"Dead Version of the {this.gameObject.name} was not set!");
        }
    }

    public virtual void SetRouteNode(Transform node) // Gets the parent route node
    {
        baseRouteNode = node;
    }

    private void SpawnDeadVersion()
    {
        if (!isAlive)
        {
            return;
        }
        this.gameObject.SetActive(false);
        Instantiate(deadVersion, transform.position, Quaternion.identity);
        isAlive = false;
    }

    public void ShowLocation()
    {
        if (miniMapSprite == null) 
        {
            Debug.Log($"Enemy \"{gameObject.name}\" does not have a miniMapSprite!");
            return;
        }

        ShowIconForSeconds(showSpriteDuration);

    }

    private async void ShowIconForSeconds(float duration)
    {
        var end = Time.time + duration;
        while (Time.time < end)
        {
            miniMapSprite.SetActive(true);
            await Task.Yield();
        }
        miniMapSprite.SetActive(false);

    }
}
