using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject detectionCone;
    bool isFacingRight;
    // Start is called before the first frame update
    void OnEnable()
    {
        int chooseFaceDirection = Random.Range(0, 2);
        if(chooseFaceDirection == 0)
        {
            isFacingRight = true;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0,90,0);

        }
        else
        {
            isFacingRight = false;
            sprite.flipX = !isFacingRight;
            detectionCone.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public bool GetFaceDirection()
    {
        return isFacingRight;
    }
}
