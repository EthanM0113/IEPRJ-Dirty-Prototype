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

            PlayerDataHolder playerDataHolder = PlayerDataHolder.Instance;

            playerDataHolder.SetSettingsData(FindObjectOfType<UIHandler>());
            SceneManager.LoadScene("LevelOne");
        }
    }

    public void ChangeLevels()
    {
        if (LevelName == "LevelOne")
        {
            SceneManager.LoadScene("LevelTwo");
        }
        else if (LevelName == "LevelTwo")
        {
            SceneManager.LoadScene("LevelThree");
        }
        else
        {
            SceneManager.LoadScene("LevelFour");
        }
    }

}
