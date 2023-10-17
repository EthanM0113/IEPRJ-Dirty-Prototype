using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class RoomOptimization : MonoBehaviour
{
    [SerializeField] GameObject MapNode;
    
    [SerializeField] bool NeedsIcon = false;
    [SerializeField] private bool IsActive = false;
    [SerializeField] private bool HasEnemy = false;
    [SerializeField] private LayerMask PlayerLayer;
    private BaseEnemy[] EnemyContainer;
    private Transform[] childList;
    private float DetectionRadius = 25.0f;
    private bool HasExplored = false;

    [SerializeField] private GameObject EnteredRoomSprite;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("GetChildrenList", 0.2f);

        if (EnteredRoomSprite == null)
        {
            Debug.Log($"{this.gameObject.name} doesn't have a Room Sprite!");
        }
        else
        {
            EnteredRoomSprite.SetActive(false);

        }
    }

    
    private void Update()
    {
        if (HasEnemy)
        {
            EnemyContainer = gameObject.GetComponentsInChildren<BaseEnemy>();
        }
        else if (!HasEnemy)
        {
            childList = gameObject.GetComponentsInChildren<Transform>();

        }
        Collider[] Player = Physics.OverlapSphere(transform.position, DetectionRadius, PlayerLayer);
        if (Player.Length == 0) //If there is no player nearby
        {
            if (HasEnemy)
            {
                for (int i = 0; i < EnemyContainer.Length; i++)
                {
                    EnemyContainer[i].PauseEnemy();
                }

            }
            //else if (!HasEnemy)
            //{
            //    for (int i = 3; i < childList.Length; i++)
            //    {
            //        childList[i].gameObject.SetActive(false);
            //    }
            //}
            
            IsActive = false;
            return;
        }
        IsActive = true;
        if (HasEnemy)
        {
            for (int i = 0; i < EnemyContainer.Length; i++)
            {
                EnemyContainer[i].ResumeEnemy();
            }

        }
        //else if (!HasEnemy)
        //{
        //    for (int i = 3; i < childList.Length; i++)
        //    {
        //        childList[i].gameObject.SetActive(true);
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("MapNode").transform.position = MapNode.transform.position;
            if (!HasExplored && NeedsIcon)
            {
                MapNode.transform.GetChild(0).gameObject.SetActive(true);
            }

            if (EnteredRoomSprite != null)
                EnteredRoomSprite.SetActive(true);

        }
    }



    /*private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("RespawnNode").transform.position = collision.gameObject.transform.position;
        }

    }*/

}
