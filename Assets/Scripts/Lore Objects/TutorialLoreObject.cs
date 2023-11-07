using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialLoreObject : MonoBehaviour
{
    [SerializeField] int gateRoomNumber;
    [SerializeField] string textInput;
    private GameObject lore;
    private TutorialManager tutorialManager;

    void Awake()
    {
        lore = GameObject.FindGameObjectWithTag("LoreText");
        tutorialManager = FindObjectOfType<TutorialManager>();      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gateRoomNumber == tutorialManager.GetCurrentRoom())
        {
            TextAppear();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TextFade();
        }
    }

    public void SetText(string text)
    {
        textInput = text;
    }

    public void UpdateText(string text)
    {
        textInput = text;
        TextFade();
        Invoke("TextAppear", 0.2f);
    }

    private void TextFade()
    {
        lore.GetComponent<Animator>().SetBool("PlayerEnter", false);
        lore.GetComponent<Animator>().SetBool("PlayerLeft", true);

    }

    private void TextAppear()
    {
        lore.GetComponent<TextMeshProUGUI>().text = textInput;
        lore.GetComponent<Animator>().SetBool("PlayerEnter", true);
        lore.GetComponent<Animator>().SetBool("PlayerLeft", false);
    }
}
