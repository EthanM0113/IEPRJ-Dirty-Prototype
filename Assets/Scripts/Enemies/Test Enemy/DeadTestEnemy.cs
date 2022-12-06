using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTestEnemy : MonoBehaviour
{
    // Objects lifetime 
    [SerializeField] float lifetime = 5f;
    float lifeTimer;

    bool gettingConsumed = false;

    // How long it takes to consume
    [SerializeField] float consumeTime = 1f;
    float consumeTimer;

    [SerializeField] ParticleSystem consumeParticle;
    [SerializeField] Ability.Type consumedAbility;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifetime;
        consumeTimer = consumeTime;
        consumeParticle.Play();
        
    }

    // Update is called once per frame
    void Update()
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

    public void SetConsumed(bool isConsuming)
    {
        gettingConsumed = isConsuming;
    }

    public Ability.Type Consumed()
    {
        if (consumeTimer <= 0)
        {
            Destroy(this.gameObject, .5f);
            return consumedAbility;
        }
        else
        {
            return Ability.Type.NONE;
        }
    }

    public void AddInfluence(ParticleSystemForceField field)
    {
        consumeParticle.externalForces.AddInfluence(field);
    }

    public void RemoveInfluence()
    {
        consumeParticle.externalForces.RemoveAllInfluences();
    }
}
