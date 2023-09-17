using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int OpeningDirection;
    //1 needs a bottom door
    //2 needs a right door
    //3 needs a left door
    //4 needs a top door

    private RoomTemplates templates;
    private int rand;
    private GameObject room;
    

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    private void Spawn()
    {
        rand = Random.Range(0, templates.Templates.Length); //Selects a random interior
        room = Instantiate(templates.BaseRoom, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("RoomList").transform); //Spawns the base room then sets parent to Roomlist
        Instantiate(templates.Templates[rand], transform.position, templates.Templates[rand].transform.rotation, room.transform); //Spawns interior inside base room then sets parent to base room
        room.GetComponent<RoomProperties>().RoomSetup(OpeningDirection); //Sets up the room
        
    }
}
