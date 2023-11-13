using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotManager : MonoBehaviour
{
    [SerializeField] private float LIFE_DURATION = 2.0f;
    private float ticks = 0.0f;
    private Animator mainCameraAnimator;
    private int killReward;
    private PlayerMoneyUIHandler playerMoneyUIHandler;
    [SerializeField] private GameObject burnParticles;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraAnimator = Camera.main.GetComponent<Animator>();
        playerMoneyUIHandler = FindObjectOfType<PlayerMoneyUIHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        ticks += Time.deltaTime;
        if (ticks >= LIFE_DURATION)
        {
            gameObject.SetActive(false);
            ticks = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Burn normal enemies on hit
        if (other.CompareTag("TestEnemy"))
        {
            Debug.Log("Triggering Burn Enemy.");
            mainCameraAnimator.SetTrigger("isQuickZoom");
            SoundManager.Instance.Fireball();

            // Add Burn Particles
            GameObject burnParticlesObject = Instantiate(burnParticles);
            burnParticlesObject.transform.position = other.transform.position;
            Destroy(burnParticlesObject, 7); // should linger a bit more than corpse

            // Kill Enemy
            other.GetComponent<BaseEnemy>().EnemyDeath();

            // Add coins
            if (other.name.Contains("TestEnemy")) // Beholder
            {
                killReward = Random.Range(3, 5);
            }
            else if (other.name.Contains("SampleWisp")) // Wisp
            {
                killReward = Random.Range(6, 8);
            }
            playerMoneyUIHandler.PulseCointText();
            PlayerMoneyManager.Instance.AddCoins(killReward);
        }
    }
}
