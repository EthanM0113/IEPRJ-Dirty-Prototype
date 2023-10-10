using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class UISkillHandler : MonoBehaviour
{
    private float skillMeter = 0.001f;
    [SerializeField] private Texture2D m_blank;
    [SerializeField] private Texture2D m_skillIcon1;
    [SerializeField] private Texture2D m_skillIcon2;

    [SerializeField] private Image m_skillIconRef;

    Ability.Type m_setAbility = Ability.Type.NONE;
    Animator m_anim;

    bool m_isFilled = false;

    // Start is called before the first frame update
    void Start()
    {
        m_skillIconRef.material.SetColor("_Color", Color.white * 0f);
        m_skillIconRef.material.SetTexture("_Texture", m_blank);
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillMeter <= 0.9f)
        {
            float output = 1f - skillMeter;
            m_skillIconRef.material.SetColor("_Color", Color.white * output);
            m_isFilled = false;
        }
        if (skillMeter <= 0f)  
        {
            if (!m_isFilled) 
            { 
                m_isFilled = true;
            }
        }
        m_anim.SetBool("IsReady", m_isFilled);
    }

    public void SetSkillMeter(float value) 
    { 
        skillMeter = value;
    }

    public void SetAbility(Ability.Type value) 
    {
        if (value == Ability.Type.NONE)
        {
            m_skillIconRef.material.SetTexture("_Texture", m_blank);
        }
        else if (value == Ability.Type.TEST)
        {
            m_skillIconRef.material.SetTexture("_Texture", m_skillIcon1);
        }
        else if (value == Ability.Type.FLARE)
        {
            m_skillIconRef.material.SetTexture("_Texture", m_skillIcon2);
        }
    }

    public void PlayNotReady()
    {
        m_anim.SetTrigger("NotReady");

    }
}
