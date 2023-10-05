using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlotHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_skillSlotUI;
    [SerializeField] private GameObject m_consumedSkillHolder;
    private PlayerInputHandler m_InputHandler;

    bool isUIActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        m_InputHandler = FindObjectOfType<PlayerInputHandler>();
        m_skillSlotUI.SetActive(isUIActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InputHandler.IsSkillSlotMenuActive())
        {
            isUIActive = !isUIActive;
        }

        m_skillSlotUI.SetActive(isUIActive);
    }
}
