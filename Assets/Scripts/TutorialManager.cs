using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject room1Torch;
    [SerializeField] private GameObject room2GateTrigger;
    [SerializeField] private GameObject room3GateTrigger;
    int currentRoom = 1; // Start at room 1
    private HpTorchHandler hpTorchHandler;
    [SerializeField] private List<GameObject> gateList;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoom == 1) 
        {
            hpTorchHandler = room1Torch.GetComponent<HpTorchHandler>();  
            if(!hpTorchHandler.GetFlameLight().activeSelf)
            {
                gateList[currentRoom - 1].SetActive(false); 
                currentRoom = 2;
            }
        }
        else if (currentRoom == 2)
        {
            TutorialGameTriggerHandler tutorialGameTriggerHandler = room2GateTrigger.GetComponent<TutorialGameTriggerHandler>();        
            if(tutorialGameTriggerHandler.GetDidTrigger())
            {
                gateList[currentRoom - 1].SetActive(false);
                currentRoom = 3;
            }
        }
        else if (currentRoom == 3)
        {
            TutorialGameTriggerHandler tutorialGameTriggerHandler = room3GateTrigger.GetComponent<TutorialGameTriggerHandler>();
            if (tutorialGameTriggerHandler.GetDidTrigger())
            {
                gateList[currentRoom - 1].SetActive(false);
                currentRoom = 4;
            }
        }



    }
}
