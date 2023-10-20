using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class LastRoomUI : MonoBehaviour
{
    [SerializeField] GameObject detectionSprite;
    [SerializeField] float randomOffset = 5.0f;
    [SerializeField] float detectionDuration = 5.0f;
    Animator animator;
    [SerializeField] bool isEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        detectionSprite.SetActive(true);
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", false);

    }

    public async void Detect(Vector3 position)
    {
        if (!isEnabled) return;

        detectionSprite.transform.position = new Vector3(
            Random.Range(position.x - randomOffset, position.x + randomOffset),
            0,
            Random.Range(position.z - randomOffset, position.z + randomOffset)
            );
        var end = Time.time + detectionDuration;
        while (end > Time.time)
        {
            animator.SetBool("IsOpen", true);
            await Task.Yield();
        }
        animator.SetBool("IsOpen", false);
    }
}
