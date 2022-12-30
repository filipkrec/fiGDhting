using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPick : MonoBehaviour
{
    public Image m_image;
    public Image m_border;
    public Image m_pickedImage;
    public TextMeshProUGUI m_name;

    [SerializeField] private CharactersScriptable m_scriptable;
    [SerializeField] private int m_player;

    private CharacterBase m_selectedChar;

    public void Set(CharacterBase _character)
    {
        m_image.sprite = _character.Icon;
        m_border.color = m_player == 1 ? m_scriptable.PlayerOneColor : m_scriptable.PlayerTwoColor;
        m_name.text = _character.name;

        m_selectedChar = _character;
    }

    public void Lock(bool _true)
    {
        m_pickedImage.gameObject.SetActive(_true);

        if (m_player == 1)
        {
            FightSetup.PlayerOne.Character = m_selectedChar;
        }
        else
        {
            FightSetup.PlayerTwo.Character = m_selectedChar;
        }
    }
}
