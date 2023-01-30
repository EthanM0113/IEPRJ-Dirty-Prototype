using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNumberUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentRoomNoText;
    [SerializeField] private TextMeshProUGUI maxRoomNoText;
    [SerializeField] private int currentRoomNo;
    [SerializeField] private int maxRoomNo;
    [SerializeField] private RoomGeneration roomGeneration;

    // Start is called before the first frame update
    void Start()
    {
        currentRoomNo = roomGeneration.GetRoomsGenerated();
        maxRoomNo = roomGeneration.GetBossRoomNo() + 1; // temporarily +1 just to not change room gen script
    }

    // Update is called once per frame
    void Update()
    {
        currentRoomNo = roomGeneration.GetRoomsGenerated();

        if(currentRoomNo != maxRoomNo) // if not in boss room yet
        { 
            currentRoomNoText.text = currentRoomNo.ToString();
            maxRoomNoText.text = maxRoomNo.ToString();
        }
        else // if in boss room
        {
            this.gameObject.SetActive(false);
        }
    }
}
