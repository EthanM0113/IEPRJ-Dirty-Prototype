using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeadEnemy : MonoBehaviour
{
    // Objects lifetime 
    [SerializeField] protected float lifetime = 5f;
    protected float lifeTimer;

    protected bool gettingConsumed = false;

    // How long it takes to consume
    [SerializeField] protected float consumeTime = 1f;
    protected float consumeTimer;

    [SerializeField] protected ParticleSystem consumeParticle;
    [SerializeField] protected Ability.Type consumedAbility;


    // Called on the start
    public void Initialize()
    {
        lifeTimer = lifetime;
        consumeTimer = consumeTime;
        consumeParticle.Play();
    }

    // Update is called once per frame

    public void Tick()
    {
        if (!gettingConsumed)
        {
            if (lifeTimer <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                lifeTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (consumeTimer > 0)
            {
                consumeTimer -= Time.deltaTime;
            }
        }
    }

    virtual public void SetConsumed(bool isConsuming)
    {
        gettingConsumed = isConsuming;
    }

    virtual public Ability.Type Consumed()
    {
        if (consumeTimer <= 0)
        {
            //Destroy(this.gameObject, .5f);
            this.gameObject.SetActive(false);
            return consumedAbility;
        }
        else
        {
            return Ability.Type.NONE;
        }
    }

    virtual public void AddInfluence(ParticleSystemForceField field)
    {
        consumeParticle.externalForces.AddInfluence(field);
    }

    public Ability.Type GetAbilityType() { return consumedAbility; }
}
