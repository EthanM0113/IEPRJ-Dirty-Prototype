using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstBossManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hpTorches;
    public List<int> litTorches;
    private bool isDoneSettingTorches = false;
    bool areBothTorchesLit = false;

    // Boss HP
    [SerializeField] private TextMeshProUGUI bossHpText;
    [SerializeField] private int bossHp;

    // Start is called before the first frame update
    void Start()
    {
        bossHpText.text = bossHp.ToString();

        for(int i = 0; i < hpTorches.Count; i++)
        {
            hpTorches[i].GetComponent<HpTorchHandler>().SetFlameLight(false);
        }

        LigtTwoTorches();
    }

    // Update is called once per frame
    void Update()
    {
        if(areBothTorchesLit = CheckTwoTorchesLit())
        {
            Debug.Log("Torches remain lit");
        }
        else
        {
            Debug.Log("TAKE DAMAGE!!");
            bossHp--;
            bossHpText.text = bossHp.ToString();
            LigtTwoTorches();
        }
    }

    private bool CheckTwoTorchesLit()
    {
        if (hpTorches[litTorches[0]].GetComponent<HpTorchHandler>().GetFlameLight().activeSelf ||
            hpTorches[litTorches[1]].GetComponent<HpTorchHandler>().GetFlameLight().activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LigtTwoTorches()
    {
        litTorches = new List<int>();

        while (!isDoneSettingTorches)
        {
            int pickedTorch = Random.Range(0, 4);
            Debug.Log("Picked Torch " + pickedTorch);

            if (litTorches.Count == 0)
            {
                litTorches.Add(pickedTorch);
            }
            else
            {
                if (pickedTorch != litTorches[0])
                {
                    litTorches.Add(pickedTorch);
                }
            }

            if (litTorches.Count == 2)
            {
                isDoneSettingTorches = true;
                Debug.Log("Done Setting Torches");
            }
        }

        Debug.Log("Selected Torch " + litTorches[0] + " and Torch " + litTorches[1]);

        // Light 2 Selected Torches
        hpTorches[litTorches[0]].GetComponent<HpTorchHandler>().SetFlameLight(true);
        hpTorches[litTorches[1]].GetComponent<HpTorchHandler>().SetFlameLight(true);

        // reset values
        isDoneSettingTorches = false; 
    }
}
