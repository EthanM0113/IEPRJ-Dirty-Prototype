using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerHandler : MonoBehaviour
{
    [SerializeField] private bool didTrigger;

    // Start is called before the first frame update
    void Start()
    {
        didTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            didTrigger = true;
        }
    }

    public bool GetDidTrigger()
    {
        return didTrigger;
    }
}
