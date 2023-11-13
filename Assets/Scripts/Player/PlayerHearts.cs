using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHearts : MonoBehaviour
{
    [SerializeField] private int currentHp;
    [SerializeField] private int maxHp;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private GameObject heartGameObject;
    [SerializeField] private List<GameObject> heartsList;
    [SerializeField] private Transform firstHeartTransform;
    [SerializeField] private Transform secondHeartTransform;

    [SerializeField] private AudioClip damageSound;

    private FuelBarHandler fuelBarHandler;
    private Animator playerAnimator;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;

        maxHp = PlayerDataHolder.Instance.GetMaxHealth();
        currentHp = PlayerDataHolder.Instance.GetCurrentHealth();
        UpdateMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentHealth();
    }

    public void UpdateCurrentHealth()
    {
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        for (int i = 0; i < heartsList.Count; i++)
        {
            if (i < currentHp)
            {
                heartsList[i].GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                heartsList[i].GetComponent<Image>().sprite = emptyHeart;
            }

            if (i < maxHp)
            {
                heartsList[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                heartsList[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    public void UpdateMaxHealth()
    {
        for (int i = 0; i < heartsList.Count; i++)
        {
            Destroy(heartsList[i]); 
        }
        heartsList.Clear();
       
        float spriteInterval = Vector3.Distance(firstHeartTransform.position, secondHeartTransform.position);
        for(int i = 0; i < maxHp; i++)
        {
            heartsList.Add(GameObject.Instantiate(heartGameObject, transform));
            // Draw First Heart
            if(i == 0) 
            {
                heartsList[i].transform.position = firstHeartTransform.position;
            }
            else
            {
                Vector3 spritePos = new Vector3(firstHeartTransform.position.x + (spriteInterval * i), firstHeartTransform.position.y, firstHeartTransform.position.z);
                heartsList[i].transform.position = spritePos;

            }
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHp += amount;
        currentHp = maxHp;
        UpdateMaxHealth();
    }

    public void DamagePlayer(int dmg)
    {     
        StartCoroutine(TriggerImpactFrame()); // Hollow Knight Damage Implementation

        // Nerf player fuel and deal dmg
        fuelBarHandler = FindObjectOfType<FuelBarHandler>();
        fuelBarHandler.ResetFuel(1.0f);

        // Reduce Player HP
        SoundManager.Instance.PlaySound(damageSound);
        currentHp -= dmg;
    }

    public IEnumerator TriggerImpactFrame()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();

        playerAnimator.SetTrigger("Hit");
        playerController.PlayDeathParticles();
        yield return new WaitForSecondsRealtime(0.30f); // Fine tune to fill in the animation
        playerAnimator.SetTrigger("Idle");
        Time.timeScale = 1;
    }

    public int GetCurrentHp()
    {
        return currentHp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }
}
