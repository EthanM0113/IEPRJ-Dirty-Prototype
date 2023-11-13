using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    // Singleton Instance
    public static PlayerMoneyManager Instance;

    // Private Variables
    [SerializeField] private int coins = 0;
    [SerializeField] private int initialCoins = 20;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        coins = initialCoins;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void SubtractCoins(int amount)
    {
        coins -= amount;
    }

    public void ResetCoins()
    {
        coins = initialCoins;
    }
}
