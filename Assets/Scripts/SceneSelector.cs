using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSelector : MonoBehaviour
{
    [SerializeField] string LevelName; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && LevelName == "Tutorial")
        {
            SceneManager.LoadScene("LevelOne");
        }
    }

    public void ChangeLevels()
    {
        if (LevelName == "LevelOne")
        {
            SceneManager.LoadScene("LevelTwo");
        }
    }
}
