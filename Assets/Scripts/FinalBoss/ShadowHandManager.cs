using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float handTicks = 0f;
    [SerializeField] private float handDuration;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        handTicks += Time.deltaTime;
        if (handTicks > handDuration)
        {
            TriggerDamage();
        }

    }

    public void TriggerDamage()
    {
        // Damage Player
        Destroy(gameObject);
    }
}
