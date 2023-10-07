using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShopManager : MonoBehaviour
{
    // Future fixes - turn off player movement while interacting, turn off player audio (stabbing and stuff) if we decide to keep U as both attack and interact button

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
    private MainCameraManager mainCameraManager;
    private bool isCamearaTransitioning;
    private bool isShopCameraActive; 
    [SerializeField] private float transitionSpeed = 1.0f;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    [SerializeField] private PlayerHearts playerHearts;
    private PlayerController playerController;
    private PlayerInputHandler playerInputHandler;
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
        playerController = FindAnyObjectByType<PlayerController>(); 
        playerInputHandler = FindAnyObjectByType<PlayerInputHandler>(); 

        isPlayerInteractingWithShop = false;
        finishedPurchase = false;

        minimapCamera = GameObject.FindGameObjectWithTag("MapNode");
        mainCamera = Camera.main;
        originalCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);
        originalCameraRotation = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);

        mainCameraManager = FindAnyObjectByType<MainCameraManager>();
        isCamearaTransitioning = false;
        isShopCameraActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCamearaTransitioning)
        {
            UpdatePrices();
            ShopInteratction();
            DisplayShopUI();
            DisplayShopPopup();
        }
        else
        {
            if (!isShopCameraActive)
            {
                //playerController.SetCanInput(false);
                //playerInputHandler.SetCanInput(false);

                // Stop Movement Keys Here

                ChangeToShopView();
            }
                
            else
            {
               // playerController.SetCanInput(true);
                //playerInputHandler.SetCanInput(true);
                ChangeToOriginalView();
            }
                
        }
    }

    private void ShopInteratction()
    {
        if(isPlayerInteractingWithShop)
        {
            if (Input.GetKeyDown(KeyCode.U)) // Buy Max Hp Increase
            {
                bool successfulPurchase = BuyItem(hpIncreasePrice);
                if (successfulPurchase)
                {
                    SoundManager.Instance.PurchaseSuccess();
                    shopKeeperAnimator.SetTrigger("didSucceed");
                    playerHearts.IncreaseMaxHealth(1);
                    shopUI.SetActive(false);
                    isPlayerInteractingWithShop = false;
                    finishedPurchase = true;

                    //ChangeToOriginalView();
                    isCamearaTransitioning = true;
                }
                else
                {
                    SoundManager.Instance.PurchaseFail();
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
                    SoundManager.Instance.PurchaseSuccess();
                    shopKeeperAnimator.SetTrigger("didSucceed");
                    fuelBarHandler.IncreaseMaxFuel(fuelIncrease);
                    shopUI.SetActive(false);
                    isPlayerInteractingWithShop = false;

                    //ChangeToOriginalView();
                    isCamearaTransitioning = true;
                }
                else
                {
                    SoundManager.Instance.PurchaseFail();
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                shopUI.SetActive(false);
                isPlayerInteractingWithShop = false;

                ChangeToOriginalView();
                isCamearaTransitioning = true;
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
                isCamearaTransitioning = true;

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
        mainCameraManager.ToggleBlackBars(true);

        mainCamera.transform.eulerAngles = shopCameraTransformObject.transform.eulerAngles;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, shopCameraTransformObject.transform.position, ref velocity, transitionSpeed);
        
        playerUI.SetActive(false);
        minimapCamera.SetActive(false);

        // Turn off Player and Enemies
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("TestEnemy"));


        if (Vector3.Distance(mainCamera.transform.position, shopCameraTransformObject.transform.position) <= 0.1f)
        {
            isCamearaTransitioning = false;
            isShopCameraActive = true;         
        }    
    }

    private void ChangeToOriginalView()
    {
        mainCameraManager.ToggleBlackBars(false);

        mainCamera.transform.eulerAngles = originalCameraRotation;
        mainCamera.transform.localPosition = Vector3.SmoothDamp(mainCamera.transform.localPosition, originalCameraPos, ref velocity, transitionSpeed);

        playerUI.SetActive(true);
        minimapCamera.SetActive(true);

        // Turn on Player and Enemies
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("TestEnemy");

       
        if (Vector3.Distance(mainCamera.transform.localPosition, originalCameraPos) <= 0.1f)
        {
            isCamearaTransitioning = false;
            isShopCameraActive = false;
        }
    }



   
       
}

