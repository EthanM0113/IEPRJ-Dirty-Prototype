using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int OpeningDirection;
    //1 needs a bottom door
    //2 needs a top door
    //3 needs a left door
    //4 needs a right door

    private RoomTemplates templates;
    private int rand;
    private GameObject room;
    private bool isClosed;
    [SerializeField] private bool isSpawned = false;
    

    private void Awake()
    {
        
        
    }
    public void StartRoomGen()
    {
        Invoke("Spawn", 0.1f);
    }
    private void Spawn()
    {
        if (isSpawned == false) {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            if (templates.GetComponent<RoomTemplates>().GetMaxRooms() > 0) //There is still room to add rooms
            {
                if (templates.GetComponent<RoomTemplates>().GetDeadEnds() > 0) //Should not be a dead end
                {
                    isClosed = false;
                    templates.GetComponent<RoomTemplates>().DecrementCD();
                    templates.GetComponent<RoomTemplates>().DecrementMaxRooms();
                }
                else //Even if there are still excess rooms, try to spawn a dead end
                {
                    rand = Random.Range(0, 101);
                    if (rand > 74) //spawns dead end
                    {
                        isClosed = true;
                        templates.GetComponent<RoomTemplates>().ResetCD();
                    }
                    else 
                    {
                        isClosed = false;
                        templates.GetComponent<RoomTemplates>().DecrementMaxRooms();
                    }

                }
            }

            else if (templates.GetComponent<RoomTemplates>().GetMaxRooms() <= 0) //No more rooms, excess will be turned into dead ends
            {
                isClosed = true;
            }


            rand = Random.Range(0, templates.Templates.Length); //Selects a random interior
            room = Instantiate(templates.BaseRoom, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("RoomList").transform); //Spawns the base room then sets parent to Roomlist
            Instantiate(templates.Templates[rand], transform.position, templates.Templates[rand].transform.rotation, room.transform); //Spawns interior inside base room then sets parent to base room                                                                                                                                          //Instantiate(templates.Templates[rand], transform.position, templates.Templates[rand].transform.rotation, room.transform); //Spawns interior inside base room then sets parent to base room
            room.GetComponent<RoomProperties>().RoomSetup(OpeningDirection, isClosed); //Sets up the room
            isSpawned = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SpawnNode"))
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            if (other.GetComponent<RoomSpawner>().isSpawned == false && isSpawned == false)
            {
                Debug.Log("Execute delete");
                Instantiate(templates.ClosedRoom, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("RoomList").transform); //spawns the base room then sets parent to roomlist
                Destroy(gameObject);
            }
            isSpawned = true;
            //this.gameObject.transform.parent.GetComponent<ToggleOpening>().OpenWallV1();
            //other.gameObject.transform.parent.GetComponent<ToggleOpening>().OpenWallV1();
            //Destroy(gameObject);

        }
    }
}
