using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap2 : MonoBehaviour
{
    [SerializeField] private GameObject[] rows;
    bool activate = true;
    bool outerRow = true;

    void Awake()
    {
        for(int i  = 0; i < rows.Length; i++)
        {
            rows[i].SetActive(false);
        }
    }

    void Update()
    {
        if (activate) {
            activate = false;
            RetractRow();
            Invoke("ActivateRow", 1);
        }
    }

    void ActivateRow() {
        if (outerRow)
        {
            rows[0].gameObject.SetActive(true);
            rows[3].gameObject.SetActive(true);
            Debug.Log("Outer Row activated");
        }
        else
        {
            rows[1].gameObject.SetActive(true);
            rows[2].gameObject.SetActive(true);
        }

        Invoke("SwitchConfig", 1);
    }

    void RetractRow() {
        if (!outerRow)
        {
            rows[0].gameObject.SetActive(false);
            rows[3].gameObject.SetActive(false);
            Debug.Log("Inner Row retracted");
        }
        else
        {
            rows[1].gameObject.SetActive(false);
            rows[2].gameObject.SetActive(false);
        }
    }

    void SwitchConfig()
    {
        activate = true;
        outerRow = !outerRow;
    }
}
