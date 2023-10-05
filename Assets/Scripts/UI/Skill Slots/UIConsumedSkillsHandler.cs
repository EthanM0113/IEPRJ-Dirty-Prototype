using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConsumedSkillsHandler : MonoBehaviour
{
    PlayerAbilityHandler m_AbilityHandler;
    [SerializeField] UISkillIcon m_slot1;
    [SerializeField] UISkillIcon m_slot2;
    [SerializeField] UISkillIcon m_slot3;

    [SerializeField] GameObject m_consumedSkillIconPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        m_AbilityHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilityHandler>();
    }


    void OnEnable()
    {
        foreach (Transform child in transform) { Destroy(child.gameObject); }
        m_AbilityHandler.SelectSkillSlot(-1);

        for (int i = 0; i < m_AbilityHandler.GetConsumedAbilities().Count; i++)
        {
            GameObject icon = Instantiate(m_consumedSkillIconPrefab, this.transform);
            icon.GetComponent<UISkillIcon>().Initialize(m_AbilityHandler.GetConsumedAbilities()[i].ability);
        }

    }

    public UISkillIcon GetSlot1() {  return m_slot1; }
    public UISkillIcon GetSlot2() {  return m_slot2; }
    public UISkillIcon GetSlot3() {  return m_slot3; }
}
