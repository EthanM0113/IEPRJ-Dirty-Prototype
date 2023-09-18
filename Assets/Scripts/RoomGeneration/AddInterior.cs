using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AddInterior : MonoBehaviour
{
    private RoomTemplates templates;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.interiors.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
