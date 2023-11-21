using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static PlayerAbilityHandler;

public class PlayerDataHolder : MonoBehaviour
{
    PlayerAbilityHandler abilityHandler;
    PlayerHearts heartHandler;
    PlayerController fuelHandler;

    [SerializeField] int defaultMaxHP = 3;
    float defaultMaxFuel = 100f;

        int currentHP = 3,
            maxHP = 3;
    float maxFuel = 100f;
    int skillSlotCount = 1;
    public static PlayerDataHolder Instance;

    [SerializeField] float currentMusicVolume = 1f;
    [SerializeField] float currentColor = 0.7f;
    [SerializeField] float currentSFXVolume = 1f;

    int timesAtMainMenu = 0;

    List<AbilityStats> consumedAbilities = new List<AbilityStats>();
    List<Ability.Type> currentAbility = new List<Ability.Type>();
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        maxHP = defaultMaxHP; 
        maxFuel = defaultMaxFuel;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            maxHP = defaultMaxHP;
            maxFuel = defaultMaxFuel;
            //Debug.Log("On Main Menu");
            consumedAbilities.Clear();
            currentAbility.Clear();
            skillSlotCount = 1;
            timesAtMainMenu++;
            if (timesAtMainMenu > 1)
            {
                if (FindObjectOfType<MainMenuHandler>())
                {
                    FindObjectOfType<MainMenuHandler>().ActivateExitBTN();
                }
            }
        }
        else if (scene.name == "LevelOne")
        {
            maxHP = defaultMaxHP;
            //maxFuel = defaultMaxFuel;
            //Debug.Log("On Level 1");
            consumedAbilities.Clear();
            currentAbility.Clear();
            skillSlotCount = 1;
            PlayerMoneyManager.Instance.ResetCoins();
        }
    }
    
    public void SetAbilitiesReference(List<AbilityStats> abilities, List<Ability.Type> current)
    {
        consumedAbilities.Clear();
        consumedAbilities = new List<AbilityStats>(abilities);
        currentAbility = new List<Ability.Type>(current);
        //foreach (AbilityStats ability in abilities)
        //{
        //    consumedAbilities.Add(ability);
        //}
    }

    public List<AbilityStats> GetAbilitiesReference()
    {
        return consumedAbilities;
    }

    public List<Ability.Type> GetCurrentAbilities()
    {
        if (currentAbility.Count == 0)
        {
            currentAbility.Add(Ability.Type.NONE);
        }
        return currentAbility;
    }

    public void SetHealthReference(PlayerHearts reference)
    {
        if (reference == null) return;

        heartHandler = reference;
        currentHP = heartHandler.GetCurrentHp();
        maxHP = heartHandler.GetMaxHp();
    }

    public int GetMaxHealth()
    {
        return maxHP;
    }

    public int GetCurrentHealth()
    {
        return currentHP;
    }

    public void SetPlayerReference(PlayerController reference)
    {
        if (reference == null) return;

        fuelHandler = reference;
        maxFuel = fuelHandler.GetMaxFuel();
    }

    public float GetMaxFuel()
    {
        return maxFuel;
    }

    public void SetSettingsData(UIHandler handler)
    {
        currentColor = handler.brightnessSlider.value;
        currentMusicVolume = handler.musicSlider.value;
        currentSFXVolume = handler.sfxSlider.value;
    }

    public float GetCurrentSFXVolume()
    {
        return currentSFXVolume;
    }
    public float GetCurrentMusicVolume()
    {
        return currentMusicVolume;
    }

    public float GetCurrentColor()
    {
        return currentColor;
    }
    public void SetSkillSlotCount(int slotCountVal)
    {
        skillSlotCount = slotCountVal;
    }

    public int GetSkillSlotCount()
    {
        return skillSlotCount;
    }
}
