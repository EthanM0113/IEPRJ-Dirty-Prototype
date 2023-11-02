using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomScript : MonoBehaviour
{
    Collider collider;
    private void Start()
    {
        collider = GetComponent<Collider>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.U))
        {
            PlayerDataHolder playerDataHolder = PlayerDataHolder.Instance;

            playerDataHolder.SetAbilitiesReference(
                FindObjectOfType<PlayerAbilityHandler>().GetConsumedAbilities(), 
                FindObjectOfType<PlayerAbilityHandler>().GetCurrentAbilities()
                );
            playerDataHolder.SetHealthReference(FindObjectOfType<PlayerHearts>());
            playerDataHolder.SetPlayerReference(FindObjectOfType<PlayerController>());

            SceneSelector sceneSelector = GetComponent<SceneSelector>();
            sceneSelector.ChangeLevels();
        }
    }
}
