using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
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
            GameObject[] templateList = templates.SneakTemplates;
            int current = templates.GetTotalRooms() - templates.GetMaxRooms(); //Used to get what number the room is
            Debug.Log("Current: " + current + " MaxRooms: " + templates.GetMaxRooms() + " TotalRooms: " + templates.GetTotalRooms());
            
            if (current < templates.GetTotalRooms()) //if the room number is less than the max
            {
                if (current % 3 == 0 && templates.GetPity(1) == false && templates.GetPity(2) == false) //If pity hasn't been hit and it's the 3rd room room in sequence
                {
                    if (templates.GetLastSpawned() == true)  //if last spawned room is an extra room
                    {
                        templates.TogglePity(1, true); //guarantee puzzle
                        templates.TogglePity(3, false);
                    }
                    else //guarantee an extra room
                    {
                        templates.TogglePity(2, true);
                        templates.TogglePity(3, false);
                    }
                }

                rand = Random.Range(0, 101);
                if (rand < 70 && templates.GetPity(1) == false && templates.GetPity(2) == false) //Anything below 70 and if the pity isn't hit
                {
                    templateList = templates.SneakTemplates;
                }
                else //Hitting that 30% chance or guarantee
                {
                    if (templates.GetPity(1) == true) //guarantees puzzle
                    {
                        templateList = templates.PuzzleTemplates;
                        Debug.Log("guarantee" + current);
                    }
                    else if (templates.GetPity(2) == true) //guarantees extra
                    {
                        templateList = templates.ExtraTemplates;
                        Debug.Log("guarantee" + current);
                    }
                    else
                    {
                        rand = Random.Range(0, 101); //choose between puzzle or extra
                        if (rand < 50)
                        {
                            templateList = templates.PuzzleTemplates;
                        }
                        else
                        {
                            templateList = templates.ExtraTemplates;
                        }
                        Debug.Log("random" + current);
                    }
                    templates.TogglePity(1, false);
                    templates.TogglePity(2, false);

                }
            }
            //If past the max, everything else is converted into a sneak room
            rand = Random.Range(0, templateList.Length); //Selects a random interior
            room = Instantiate(templates.BaseRoom, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("RoomList").transform); //Spawns the base room then sets parent to Roomlist
            Instantiate(templateList[rand], transform.position, templateList[rand].transform.rotation, room.transform); //Spawns interior inside base room then sets parent to base room                                                                                                                                          //Instantiate(templates.Templates[rand], transform.position, templates.Templates[rand].transform.rotation, room.transform); //Spawns interior inside base room then sets parent to base room
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
