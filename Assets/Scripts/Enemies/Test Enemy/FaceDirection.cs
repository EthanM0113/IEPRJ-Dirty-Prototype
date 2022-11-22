using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    bool isFacingRight;
    // Start is called before the first frame update
    void OnEnable()
    {
        int chooseFaceDirection = Random.Range(0, 2);
        if(chooseFaceDirection == 0)
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
        }
        else
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
        }
    }

    public bool GetFaceDirection()
    {
        return isFacingRight;
    }
}
