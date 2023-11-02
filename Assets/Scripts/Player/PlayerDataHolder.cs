using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerAbilityHandler;

public class PlayerDataHolder : MonoBehaviour
{
    PlayerAbilityHandler abilityHandler;
    PlayerHearts heartHandler;
    PlayerController fuelHandler;

    int currentHP = 3,
        maxHP = 3;
    float maxFuel = 50.0f;

    public static PlayerDataHolder Instance;


    List<AbilityStats> consumedAbilities = new List<AbilityStats>();
    List<Ability.Type> currentAbility = new List<Ability.Type>();
    // Start is called before the first frame update
    void Start()
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
