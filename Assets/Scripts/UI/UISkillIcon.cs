using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UISkillIcon : MonoBehaviour
{
    [System.Serializable]
    private class Icon
    {
        public string name;
        public Ability.Type ability;
        public Sprite texture;
    }

    [SerializeField] private Icon[] m_iconList;
    private Icon m_icon;
    [SerializeField] private Image m_renderer;
    [SerializeField] private TMP_Text m_name;

    private PlayerAbilityHandler m_playerAbilities;
    private UIConsumedSkillsHandler m_UISkillHandler;



    // Start is called before the first frame update
    void Awake()
    {
        m_playerAbilities = FindObjectOfType<PlayerAbilityHandler>();
        m_UISkillHandler = FindObjectOfType<UIConsumedSkillsHandler>();
        Initialize(Ability.Type.NONE);
        
    }

    private void Update()
    {

    }

    public void Initialize(Ability.Type ability)
    {
        int index = FindIcon(ability);
        if (index > -1)
        {
            m_icon = m_iconList[index];

            m_renderer.sprite = m_icon.texture;
            m_name.SetText(m_icon.name);
        }
        else
        {
            Debug.Log($"Icon for {ability} no found! Script UISkillIcon.cs");
        }

    }
    
    // If the icon exists then return the index else return -1
    int FindIcon(Ability.Type ability)
    {
        for(int i = 0; i < m_iconList.Length; i++) 
        {
            if (m_iconList[i].ability == ability)
            {
                return i;
            }
        }
        Debug.Log($"Warning! Icon for ability \"{ability.ToString()}\" could not be found!");
        return -1;
    }

    public void SetAbility()
    {
        int index = m_playerAbilities.SetCurrentAbility(m_icon.ability);
        if (index == 1)
        {
            m_UISkillHandler.GetSlot1().Initialize(m_icon.ability);
        }
        else if (index == 2)
        {
            m_UISkillHandler.GetSlot2().Initialize(m_icon.ability);
        }
        else if (index == 3)
        {
            m_UISkillHandler.GetSlot3().Initialize(m_icon.ability);
        }
    }

    public void AssignSkillSlot(int index)
    {
        m_playerAbilities.SelectSkillSlot(index);
    }
}
