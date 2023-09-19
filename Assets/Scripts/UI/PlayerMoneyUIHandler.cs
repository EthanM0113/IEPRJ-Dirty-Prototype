using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int coins = PlayerMoneyManager.Instance.GetCoins();
        coinText.SetText(coins.ToString());
    }
}
