using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadTestEnemy : BaseDeadEnemy
{
    bool isInTutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            isInTutorial = true;
        }
    }

    private void Update()
    {
        if (isInTutorial)
        
            TutorialTick();
        
        else
            Tick();
    }
}
