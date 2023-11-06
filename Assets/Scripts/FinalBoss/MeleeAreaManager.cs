 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    Color activeTint;
    [SerializeField] private float attackSpeed;
    PlayerHearts playerHearts;
    private bool isPlayerColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        activeTint = Color.white;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = activeTint;
        playerHearts = FindAnyObjectByType<PlayerHearts>();
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
        if (isPlayerColliding)
        {
            playerHearts.DamagePlayer(3);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false;
        }
    }
}
