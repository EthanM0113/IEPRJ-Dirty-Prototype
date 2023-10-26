using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
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
    }

    public void SetWater(bool hasWater) { water = hasWater; }

    public bool GetWater() { return water; }

    public void SetChalice(bool hasChalice) {  chalice = hasChalice; }

    public bool GetChalice() { return chalice; }

    public void SetKey(bool hasKey) { rustedKey  = hasKey; }

    public bool GetKey() { return rustedKey; }

    public void SetBread(int currBread) { moldyBread = currBread; }

    public void SetIceChunks(bool hasIceChunks) {  iceChunks = hasIceChunks; }

    public void SetFreezingScroll(bool hasFreezingScroll) { freezingScroll = hasFreezingScroll; }

    public void SetGear(bool hasGear) { grandDoorGear = hasGear; }
}
