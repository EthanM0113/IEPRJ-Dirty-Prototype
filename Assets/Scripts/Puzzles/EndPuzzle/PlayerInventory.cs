using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryText;

    private bool water;
    private bool chalice;
    private bool rustedKey;
    private int moldyBread;
    private bool iceChunks;
    private bool freezingScroll;
    private bool grandDoorGear;

    void Awake()
    {
        water = false;
        chalice = false;
        rustedKey = false;
        moldyBread = 0;
        iceChunks = false;
        freezingScroll = false;
        grandDoorGear = false;

        inventoryText.GetComponent<TextMeshProUGUI>().text = "Inventory: \n";
    }

    private void UpdateList()
    {
        inventoryText.GetComponent<TextMeshProUGUI>().text = "Inventory: \n";

        if (rustedKey)
            inventoryText.GetComponent<TextMeshProUGUI>().text += "Rusted Key\n";
        if(moldyBread > 0)
        {
            for (int i = 0; i < moldyBread; i++)
                inventoryText.GetComponent<TextMeshProUGUI>().text += "MoldyBread\n";
        }
        if(iceChunks)
            inventoryText.GetComponent<TextMeshProUGUI>().text += "Chunks of Ice\n";
        if(freezingScroll)
            inventoryText.GetComponent<TextMeshProUGUI>().text += "Scroll of Freezing\n";
        if (chalice)
        {
            if(water)
                inventoryText.GetComponent<TextMeshProUGUI>().text += "Chalice full of water\n";
            else
                inventoryText.GetComponent<TextMeshProUGUI>().text += "Empty Chalice\n";
        }
        if(grandDoorGear)
            inventoryText.GetComponent<TextMeshProUGUI>().text += "Grand Door Gear\n";
    }

    public void SetWater(bool hasWater) { water = hasWater; UpdateList(); }

    public bool GetWater() { return water; }

    public void SetChalice(bool hasChalice) {  chalice = hasChalice; UpdateList(); }

    public bool GetChalice() { return chalice; }

    public void SetKey(bool hasKey) { rustedKey  = hasKey; UpdateList(); }

    public bool GetKey() { return rustedKey; }

    public void SetBread(int currBread) { moldyBread = currBread; UpdateList(); }

    public void SetIceChunks(bool hasIceChunks) {  iceChunks = hasIceChunks; UpdateList(); }

    public bool GetIceChunks() { return iceChunks; }

    public void SetFreezingScroll(bool hasFreezingScroll) { freezingScroll = hasFreezingScroll; UpdateList(); }

    public bool GetScroll() {  return freezingScroll; }

    public void SetGear(bool hasGear) { grandDoorGear = hasGear; UpdateList(); }

    public bool GetGear() {  return grandDoorGear; }
}
