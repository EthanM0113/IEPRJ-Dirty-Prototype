using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject room1Torch;
    [SerializeField] private GameObject room2GateTrigger;
    [SerializeField] private GameObject room3GateTrigger;
    private int currentRoom = 1; // Start at room 1
    private HpTorchHandler hpTorchHandler;
    [SerializeField] private List<GameObject> gateList;
    private GameObject player;

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
                gateList[currentRoom - 1].GetComponent<Animator>().SetBool("isSolved", true); 
                currentRoom = 2;
            }
        }
        else if (currentRoom == 2)
        {
            TutorialTriggerHandler tutorialTriggerHandler = room2GateTrigger.GetComponent<TutorialTriggerHandler>();        
            if(tutorialTriggerHandler.GetDidTrigger())
            {
                gateList[currentRoom - 1].GetComponent<Animator>().SetBool("isSolved", true);
                currentRoom = 3;
            }
        }
        else if (currentRoom == 3)
        {
            TutorialTriggerHandler tutorialTriggerHandler = room3GateTrigger.GetComponent<TutorialTriggerHandler>();
            if (tutorialTriggerHandler.GetDidTrigger())
            {
                gateList[currentRoom - 1].GetComponent<Animator>().SetBool("isSolved", true);
                currentRoom = 4;
            }
        }
        else if (currentRoom == 4)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            // Ability was absorbed from enemy
            if (player.GetComponent<PlayerAbilityHandler>().GetCurrentAbility() != Ability.Type.NONE)
            {
                gateList[currentRoom - 1].GetComponent<Animator>().SetBool("isSolved", true);
                currentRoom = 5;
            }
        }
        else if (currentRoom == 5)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            // Ability was switched to Wisp Ability
            if (player.GetComponent<PlayerAbilityHandler>().GetCurrentAbility() == Ability.Type.FLARE)
            {
                gateList[currentRoom - 1].GetComponent<Animator>().SetBool("isSolved", true);
                currentRoom = 6;
            }
        }

    }

    public int GetCurrentRoom()
    {
        return currentRoom; 
    }
}
