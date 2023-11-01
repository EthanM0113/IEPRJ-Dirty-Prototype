using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    Color activeTint;

    // Start is called before the first frame update
    void Start()
    {
        activeTint = Color.white;

        spriteRenderer = GetComponent<SpriteRenderer>();    
        spriteRenderer.color = activeTint;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn red over time
        activeTint.g -= Time.deltaTime;
        activeTint.b -= Time.deltaTime;
        spriteRenderer.color = activeTint;

        if (activeTint.g < 0f && activeTint.b < 0f)
        { 
            StartCoroutine(TriggerHands());
        }
    }

    public IEnumerator TriggerHands()
    {
        yield return new WaitForSeconds(3f);
        // Damage Player
        Destroy(gameObject);
    }
}
