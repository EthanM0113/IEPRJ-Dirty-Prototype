using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthSlider : MonoBehaviour
{
    Slider hp;
    [SerializeField] PlayerController playerController;

    private void Start()
    {
        hp = GetComponent<Slider>();
        SetMaxHealth(playerController.maxHealth);
        SetHealth(playerController.health);
    }

    private void Update()
    {
        SetHealth(playerController.health);
    }

    public void SetMaxHealth(int health)
    {
        this.hp.maxValue = health;
        this.hp.value = health;
    }

    public void SetHealth(int health)
    {
        hp.value = health;
    }
}
