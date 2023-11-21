using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] ParticleSystem breakEffect;
    int zeroCoinRates = 60;
    int oneCoinRates = 35; 

    int lowReward = 1;
    int highReward = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        if (breakEffect == null) return;

        breakEffect.Play();

        int random = Random.Range(0, 100);
        Debug.Log("value: " + random);
        if (random >= 0 && random < zeroCoinRates)
        {
            SoundManager.Instance.PropBreak();
        }
        else if(random >= zeroCoinRates && random < oneCoinRates + zeroCoinRates)
        {
            SoundManager.Instance.PropBreak();
            PlayerMoneyManager.Instance.AddCoins(lowReward);
            FindObjectOfType<PlayerMoneyUIHandler>().PulseCointText();
            FindObjectOfType<PlayerMoneyUIHandler>().SpinCoinImage();
        }
        else
        {
            SoundManager.Instance.PropBreak();
            PlayerMoneyManager.Instance.AddCoins(highReward);
            FindObjectOfType<PlayerMoneyUIHandler>().PulseCointText();
            FindObjectOfType<PlayerMoneyUIHandler>().SpinCoinImage();
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
