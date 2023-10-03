using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopInteractColliderHandler shopInteractColliderHandler;
    [SerializeField] private GameObject shopInteractPopup;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Animator shopKeeperAnimator;
    [SerializeField] private bool isPlayerInteractingWithShop;
    private bool finishedPurchase; // temporary flag because attack and buy for max hp is same button

    // Trying this shop style
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject shopCameraTransformObject;
    [SerializeField] private Transform originalCameraTransform;
    [SerializeField] private Vector3 originalCameraPos;
    [SerializeField] private Vector3 originalCameraRotation;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject minimapCamera;

    [SerializeField] private PlayerHearts playerHearts;
    [SerializeField] private FuelBarHandler fuelBarHandler;
    [SerializeField] private float fuelIncrease;

    // Prices
    [SerializeField] private TextMeshProUGUI hpCostText;
    [SerializeField] private TextMeshProUGUI fuelCostText;
    [SerializeField] private int hpIncreasePrice;
    [SerializeField] private int fuelIncreasePrice;

    // Start is called before the first frame update
    void Start()
    {
        playerHearts = FindAnyObjectByType<PlayerHearts>();
        fuelBarHandler = FindAnyObjectByType<FuelBarHandler>(); 
        isPlayerInteractingWithShop = false;
        finishedPurchase = false;

        minimapCamera = GameObject.FindGameObjectWithTag("MapNode");
        mainCamera = Camera.main;
        originalCameraPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        originalCameraRotation = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePrices();
        ShopInteratction();
        DisplayShopUI();
        DisplayShopPopup();
    }

    private void ShopInteratction()
    {
        if(isPlayerInteractingWithShop)
        {
            if (Input.GetKeyDown(KeyCode.U)) // Buy Max Hp Increase
            {
                bool successfulPurchase = BuyItem(hpIncreasePrice);
                if(successfulPurchase)
                {
                    SoundManager.Instance.BackstabHit(); // replace with buy sound next time
                    shopKeeperAnimator.SetTrigger("didSucceed");
                    playerHearts.IncreaseMaxHealth(1);
                    shopUI.SetActive(false);
                    isPlayerInteractingWithShop = false;
                    finishedPurchase = true;

                    ChangeToOriginalView();
                } 
            }
            if(Input.GetKeyUp(KeyCode.U)) 
            {
                finishedPurchase = false;
            }
            if (Input.GetKeyDown(KeyCode.I)) // Buy Max Fuel Increase
            {
                bool successfulPurchase = BuyItem(fuelIncreasePrice);
                if(successfulPurchase) 
                {
                    SoundManager.Instance.BackstabHit(); // replace with buy sound next time
                    shopKeeperAnimator.SetTrigger("didSucceed");
                    fuelBarHandler.IncreaseMaxFuel(fuelIncrease);
                    shopUI.SetActive(false);
                    isPlayerInteractingWithShop = false;

                    ChangeToOriginalView();
                }           
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                shopUI.SetActive(false);
                isPlayerInteractingWithShop = false;

                ChangeToOriginalView();
            }
        }
    }

    private bool BuyItem(int cost)
    {
        int currentMoney = PlayerMoneyManager.Instance.GetCoins();
        if (currentMoney - cost < 0) // broke
        {
            Debug.Log("Not enought money, need " + (cost - currentMoney) + " more coins.");
            return false;
        }
        else // well off
        {
            PlayerMoneyManager.Instance.SubtractCoins(cost);
            return true;
        }
    }

    private void DisplayShopUI()
    {
        if (shopInteractColliderHandler.GetIsPlayerWithinRange() && !isPlayerInteractingWithShop && !finishedPurchase)
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                ChangeToShopView();

                shopInteractPopup.SetActive(false);
                shopUI.SetActive(true);
                isPlayerInteractingWithShop = true;
            }
        }
        else if(!shopInteractColliderHandler.GetIsPlayerWithinRange())
        {
            shopUI.SetActive(false);
            isPlayerInteractingWithShop = false;
        }
    }

    private void DisplayShopPopup()
    {
        if(shopInteractColliderHandler.GetIsPlayerWithinRange() && !isPlayerInteractingWithShop)
        {
            finishedPurchase = false;
            shopInteractPopup.SetActive(true);
        }
        else
        {
            shopInteractPopup.SetActive(false);
        }
    }

    private void UpdatePrices()
    {
        hpCostText.SetText(hpIncreasePrice.ToString());
        fuelCostText.SetText(fuelIncreasePrice.ToString());
    }

    private void ChangeToShopView()
    {
        //originalCameraTransform = mainCamera.transform;
        originalCameraPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        originalCameraRotation = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
        mainCamera.transform.position = shopCameraTransformObject.transform.position;
        mainCamera.transform.eulerAngles = shopCameraTransformObject.transform.eulerAngles;

        playerUI.SetActive(false);
        minimapCamera.SetActive(false);

        // Turn off Player and Enemies
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("TestEnemy"));
    }

    private void ChangeToOriginalView()
    {
        mainCamera.transform.position = originalCameraPos;
        mainCamera.transform.eulerAngles = originalCameraRotation;

        playerUI.SetActive(true);
        minimapCamera.SetActive(true);

        // Turn on Player and Enemies
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("TestEnemy");
    }
}

