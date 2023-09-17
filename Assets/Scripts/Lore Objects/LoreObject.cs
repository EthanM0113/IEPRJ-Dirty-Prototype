using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreObject : MonoBehaviour
{
    [SerializeField] string textInput;

    private GameObject lore;
    void Awake()
    {
        lore = GameObject.FindGameObjectWithTag("LoreText");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") {
            lore.GetComponent<TextMeshProUGUI>().text = textInput;
            lore.GetComponent<Animator>().SetBool("PlayerEnter", true);
            lore.GetComponent<Animator>().SetBool("PlayerLeft", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            lore.GetComponent<Animator>().SetBool("PlayerEnter", false);
            lore.GetComponent<Animator>().SetBool("PlayerLeft", true);
        }
    }
}
