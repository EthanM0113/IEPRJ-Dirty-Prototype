using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WispBehaviour : BaseEnemy
{
    FaceDirection faceDirection;
    private float faceTicks = 0.0f;
    [SerializeField] private float FACE_INTERVAL = 1.0f;
    //[SerializeField] private float minFaceInterval = 0.7f; trying without random first
    //[SerializeField] private float maxFaceInterval = 1.5f;
    bool faceRight = true;
    [SerializeField] private ParticleSystem tpParticles;
    [SerializeField] private ParticleSystem tpInParticles;
    [SerializeField] private ParticleSystem tpParticleHolder;
    [SerializeField] private Light selfLight;
    private bool isRooted = false;
    [SerializeField] private AudioClip perishSFX;

    // Start is called before the first frame update
    void Awake()
    {
        faceDirection = GetComponent<FaceDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRooted)
        {
            if (isActivated)
            {
                faceTicks += Time.deltaTime;
                if (faceTicks >= FACE_INTERVAL)
                {
                    faceDirection.Flip(!faceRight);
                    faceRight = !faceRight;
                    //Random.InitState(Random.Range(int.MinValue, int.MaxValue));
                    //FACE_INTERVAL = Random.Range(minFaceInterval, maxFaceInterval);
                    faceTicks = 0.0f;
                }
            }
        }
    }

    public void PlayTpParticles(Vector3 pos)
    {
        tpParticleHolder = Instantiate(tpParticles, pos, Quaternion.identity);
        Destroy(tpParticleHolder.gameObject, 1.5f);
    }

    public void PlayPreTPParticles()
    {
        tpInParticles.Play();
    }

    public Light GetSelfLight()
    {
        return selfLight;
    }

    public void SetIsRooted(bool flag)
    {
        isRooted = flag;
    }

    public bool GetIsRooted()
    {
        return isRooted; 
    }

    public override void EnemyDeath()
    {
        SoundManager.Instance.EnemyPerish(perishSFX, 0.6f);
        base.EnemyDeath();
    }
}
