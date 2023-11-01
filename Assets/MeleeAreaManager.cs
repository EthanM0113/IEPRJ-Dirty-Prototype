using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaManager : MonoBehaviour
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
        activeTint.g -= Time.deltaTime * 2f;
        activeTint.b -= Time.deltaTime * 2f;
        spriteRenderer.color = activeTint;

        if (activeTint.g < 0f && activeTint.b < 0f)
        {
            StartCoroutine(TriggerMelee());
        }
    }

    public IEnumerator TriggerMelee()
    {
        yield return new WaitForSeconds(1f);
        // Damage Player
        Destroy(gameObject);
    }
}
