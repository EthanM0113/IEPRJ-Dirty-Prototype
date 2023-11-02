 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    Color activeTint;
    [SerializeField] private float attackSpeed;

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
        activeTint.g -= Time.deltaTime * attackSpeed;
        activeTint.b -= Time.deltaTime * attackSpeed;
        spriteRenderer.color = activeTint;

        if (activeTint.g < 0f && activeTint.b < 0f)
        {
            TriggerMelee();
        }
    }

    public void TriggerMelee()
    {
        // Damage Player
        Destroy(gameObject);
    }
}
