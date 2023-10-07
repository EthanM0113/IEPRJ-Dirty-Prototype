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

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
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
                Debug.Log("Heart " + i + " is full.");
                heartsList[i].GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                Debug.Log("Heart " + i + " is empty.");
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
        SoundManager.Instance.PlaySound(damageSound);
        currentHp -= dmg;
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
