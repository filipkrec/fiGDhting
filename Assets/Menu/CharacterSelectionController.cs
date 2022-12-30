using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] private CharactersScriptable m_scriptable;
    [SerializeField] private PlayerPick m_playerOnePick;
    [SerializeField] private PlayerPick m_playerTwoPick;

    [SerializeField] private GameObject m_picksField;
    [SerializeField] private Image m_prefab;

    private void Start()
    {
        foreach(CharacterBase character in m_scriptable.Characters)
        {
            Image img = Instantiate(m_prefab, m_picksField.transform);
            img.sprite = character.Icon;
        }

        m_playerOnePick.Set(m_scriptable.Characters[0]);
        m_playerTwoPick.Set(m_scriptable.Characters[0]);
    }

}
