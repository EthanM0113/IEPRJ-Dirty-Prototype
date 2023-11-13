using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{
    public int num;

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            SoundManager.Instance.PlayInteract();
            num++;
            if (num >= 4)
                num = 0;
        }
    }

    public int GetNum() {
        return num;
    }

    public void SetNum(int numReplace) {
        num = numReplace;
    }
}
