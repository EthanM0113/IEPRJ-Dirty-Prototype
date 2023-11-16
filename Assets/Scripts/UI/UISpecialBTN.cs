using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpecialBTN : MonoBehaviour
{
    [SerializeField] Sprite normal;
    [SerializeField] Sprite hovered;
    

    public Sprite GetNormal() { return normal; }
    public Sprite GetHovered() { return hovered; }
}
