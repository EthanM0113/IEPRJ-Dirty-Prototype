using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlotHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_skillSlotUI;
    [SerializeField] private GameObject m_consumedSkillHolder;
    private PlayerInputHandler m_InputHandler;
    private PlayerDataHolder m_playerData;
    private int m_slots = 1;

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;
    PlayerController m_player;

    [SerializeField] private GameObject levelUI;

    bool isUIActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        m_playerData = PlayerDataHolder.Instance;
        m_InputHandler = FindObjectOfType<PlayerInputHandler>();
        m_player = FindObjectOfType<PlayerController>();
        levelUI.SetActive(true);
    }

    private void Start()
    {
        slot1.SetActive(false);
        slot2.SetActive(false);
        slot3.SetActive(false);

        if (m_playerData)
        {
            m_slots = m_playerData.GetSkillSlotCount();
        }
        if (m_slots >= 1)
        {
            slot1.SetActive(true);
        }
        if (m_slots >= 2)
        {
            slot2.SetActive(true);
        }
        if (m_slots >= 3)
        {
            slot3.SetActive(true);
        }


        m_skillSlotUI.SetActive(isUIActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InputHandler.IsSkillSlotMenuActive())
        {
            isUIActive = !isUIActive;
            m_player.CanMove();
        }

        m_skillSlotUI.SetActive(isUIActive);
    }
}
