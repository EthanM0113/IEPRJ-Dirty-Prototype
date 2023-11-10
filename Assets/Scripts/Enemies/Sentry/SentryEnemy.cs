using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.Animations;
using static Unity.VisualScripting.Member;

public class SentryEnemy : BaseEnemy
{
    // Used to know the rotation direction
    enum Rotation{
        Clockwise = 0, CounterClockwise
    }

    [SerializeField] float rotationPerInterval = 45f; // How much should sentry rotate per tick
    float currentRotation = 0f;
    [SerializeField] float interval = 1f; // Every hom many seconds should the sentry rotate

    [SerializeField] Rotation direction = Rotation.Clockwise; // Rotation Direction
    float dirMultiplier = 1f; // Used for computing the direction

    [SerializeField] Transform lightSource; // Reference to the light source holder

    FaceDirection faceDir;

    float intervalTime = 0f;

    bool facingRight = true;

    [SerializeField] private SpriteRenderer spriteRef;
    [SerializeField] private Sprite[] lookSpriteList;
    int spriteIndex = 0;

    Animator anim;

    AudioSource source;
    [SerializeField] AudioClip perishSFX;
    float volumeOffset = 0.8f;

    private bool isRooted = false;

    // Start is called before the first frame update
    void Awake()
    {
        faceDir = GetComponent<FaceDirection>();
        anim = GetComponent<Animator>();

        currentState = State.STATIONARY;
        intervalTime = Time.time + interval;
        if (direction == Rotation.CounterClockwise)
        {
            dirMultiplier = -1f;
        }
        spriteRef.sprite = lookSpriteList[Mathf.Abs(spriteIndex % lookSpriteList.Length)];
        spriteIndex++;

        source = GetComponent<AudioSource>();
        source.volume = volumeOffset * SoundManager.Instance.GetSFXMultiplier();

        source.Play();

    }

    // Update is called once per frame
    void Update()
    {
       if(!isActivated)
        {
            return;
        }
       if(isAlive)
       {
            if(!isRooted)
            {
                if (intervalTime <= Time.time)
                {
                    lightSource.RotateAround(lightSource.position, Vector3.up, rotationPerInterval * dirMultiplier);
                    intervalTime = Time.time + interval;

                    currentRotation += rotationPerInterval;
                    if (currentRotation >= 360f || currentRotation <= -0.01f)
                    {
                        currentRotation = 0;
                    }

                    if (Mathf.Abs(currentRotation) == 90f)
                    {
                        facingRight = true;

                        faceDir.FlipFaceDirection(facingRight); // flip face direction // edit to not change sprite
                    }
                    else if (Mathf.Abs(currentRotation) == 270f)
                    {
                        facingRight = false;

                        faceDir.FlipFaceDirection(facingRight); // flip face direction // edit to not change sprite
                    }

                    spriteRef.sprite = lookSpriteList[Mathf.Abs(spriteIndex % lookSpriteList.Length)];
                    spriteIndex++;
                    source.volume = volumeOffset * SoundManager.Instance.GetSFXMultiplier();
                }
            }
        }
    }

    public override void EnemyDeath()
    {
        SoundManager.Instance.EnemyPerish(perishSFX);
        anim.enabled = true;
        anim.SetTrigger("OnDeath");
        base.EnemyDeath();
    }

    public override void PauseEnemy()
    {
        isActivated = false;
        source.mute = true;

    }

    public override void ResumeEnemy()
    {
        isActivated = true;
        source.mute = false;

    }

    public void SetIsRooted(bool flag) 
    {
        isRooted = flag;    
    }
}
