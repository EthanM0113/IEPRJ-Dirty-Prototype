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

    public void SetWater(bool hasWater) { water = hasWater; Debug.Log("Got water!"); }

    public bool GetWater() { return water; }

    public void SetChalice(bool hasChalice) {  chalice = hasChalice; Debug.Log("Got chalice!"); }

    public bool GetChalice() { return chalice; }

    public void SetKey(bool hasKey) { rustedKey  = hasKey; Debug.Log("Got rusted key!"); }

    public bool GetKey() { return rustedKey; }

    public void SetBread(int currBread) { moldyBread = currBread; Debug.Log("Got " + moldyBread + " bread!"); }

    public void SetIceChunks(bool hasIceChunks) {  iceChunks = hasIceChunks; Debug.Log("Got ice chunks!"); }

    public bool GetIceChunks() { return iceChunks; }

    public void SetFreezingScroll(bool hasFreezingScroll) { freezingScroll = hasFreezingScroll; Debug.Log("Got freezing scroll!"); }

    public bool GetScroll() {  return freezingScroll; }

    public void SetGear(bool hasGear) { grandDoorGear = hasGear; Debug.Log("Got gear!"); }

    public bool GetGear() {  return grandDoorGear; }
}
