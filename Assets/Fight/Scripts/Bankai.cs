using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Bankai : MonoBehaviour
{
    const int DEFAULT_BANKAI_DMG = 50;

    public static Bankai s_Instance => s_instance;

    private static Bankai s_instance;

    [SerializeField] private TextMeshProUGUI m_bankaiText;
    [SerializeField] private GameObject m_bankaiUI;
    [SerializeField] private Image m_bankaiImage;
    [SerializeField] private CharactersScriptable m_charactersScriptable;
    [SerializeField] private AudioSource m_bankaiAudioSource;

    private bool m_bankaing;
    private CharacterBase m_currentCaster;
    private CharacterBase m_currentTarget;
    private CharacterInfo m_casterInfo;

    void Start()
    {
        if(s_instance != null)
        {
            Destroy(s_instance);
        }

        s_instance = this;
    }

    public bool TryDoBankai(CharacterBase _characterBase)
    {
        if (m_bankaing) return false;
        m_bankaing = true; 

        m_currentCaster = _characterBase;
        m_currentTarget = null;

        foreach (CharacterBase character in FindObjectsByType<CharacterBase>(FindObjectsSortMode.None))
        {
            if(character != m_currentCaster)
            {
                m_currentTarget = character;
            }
        }

        m_casterInfo = m_charactersScriptable.Characters.FirstOrDefault((x) => x.Character.Name == _characterBase.Name);
        m_bankaiImage.sprite = m_casterInfo.BankaiImage;
        m_bankaiText.color = m_casterInfo.Color;
        m_bankaiAudioSource.clip = m_casterInfo.BankaiClip;
        m_bankaiAudioSource.Play();

        m_bankaiUI.SetActive(true);

        return true;
    }
    
    public void DoBankai()
    {
        if(m_casterInfo.Bankai != null)
        {
            m_casterInfo.Bankai.Bankai(m_currentCaster, m_currentTarget);
        }
        else
        {
            m_currentTarget.TakeDamage(DEFAULT_BANKAI_DMG);
        }
        m_bankaiUI.SetActive(false);
        m_bankaing = false;
    }
}
