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
    [SerializeField] private StagesScriptable m_stageScriptable;
    [SerializeField] private int m_player;

    private int m_selectedCharIndex = -1;

    public int SelectedCharIndex => m_selectedCharIndex;
    public bool IsLocked => m_pickedImage.isActiveAndEnabled;

    public void Set(int _index)
    {
        CharacterBase character = m_scriptable.Characters[_index];

        m_image.sprite = character.Icon;
        m_border.color = m_player == 0 ? m_scriptable.PlayerOneColor : m_scriptable.PlayerTwoColor;
        m_name.text = character.name;

        m_selectedCharIndex = _index;
    }

    public void SetStage(int _index)
    {
        Sprite stage = m_stageScriptable.stages[_index];

        m_image.sprite = stage;
        m_border.color = m_player == 0 ? m_scriptable.PlayerOneColor : m_scriptable.PlayerTwoColor;
        m_name.gameObject.SetActive(false);

        m_selectedCharIndex = _index;
    }

    public void Lock(bool _true)
    {
        m_pickedImage.gameObject.SetActive(_true);

        if (m_player == 0)
        {
            FightSetup.PlayerOne.Character = m_scriptable.Characters[m_selectedCharIndex];
        }
        else
        {
            FightSetup.PlayerTwo.Character = m_scriptable.Characters[m_selectedCharIndex];
        }
    }
}
