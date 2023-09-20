using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopInteractColliderHandler shopInteractColliderHandler;
    [SerializeField] private GameObject shopInteractPopup;
    [SerializeField] private GameObject shopUI;
    private bool isPlayerInteractingWithShop;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerInteractingWithShop = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayShopPopup();
        DisplayShopUI();
        ShopInteratction();
    }

    private void ShopInteratction()
    {
        if(isPlayerInteractingWithShop)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.I))
            {

            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                shopUI.SetActive(false);
                isPlayerInteractingWithShop = false;
            }
        }
    }

    private void DisplayShopUI()
    {
        if (shopInteractColliderHandler.GetIsPlayerWithinRange())
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                shopInteractPopup.SetActive(false);
                shopUI.SetActive(true);
                isPlayerInteractingWithShop = true;
            }
        }
        else
        {
            shopUI.SetActive(false);
            isPlayerInteractingWithShop = false;
        }
    }

    private void DisplayShopPopup()
    {
        if(shopInteractColliderHandler.GetIsPlayerWithinRange() && !isPlayerInteractingWithShop)
        {
            shopInteractPopup.SetActive(true);
        }
        else
        {
            shopInteractPopup.SetActive(false);
        }
    }
}

