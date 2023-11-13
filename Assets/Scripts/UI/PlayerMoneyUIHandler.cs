using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Animator coinTextAnimator;

    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerMoneyManager.Instance.GetCoins();
        coinText.SetText(coins.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        int coins = PlayerMoneyManager.Instance.GetCoins();
        coinText.SetText(coins.ToString());
    }

    public void PulseCointText()
    {
        coinTextAnimator.SetTrigger("didPulse");
    }

}
