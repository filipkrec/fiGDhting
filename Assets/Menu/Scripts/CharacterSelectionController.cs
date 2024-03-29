using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour
{
    public const int FIELD_WIDTH = 4;

    [SerializeField] private CharactersScriptable m_scriptable;
    [SerializeField] private PlayerPick m_playerOnePick;
    [SerializeField] private PlayerPick m_playerTwoPick;

    [SerializeField] private GameObject m_picksField;
    [SerializeField] private Selection m_prefab;
    [SerializeField] private StageSelection m_stageSelection;

    private List<Selection> m_characterSelections = new();
    private int m_rowCount;

    private void Start()
    {
        foreach (CharacterInfo character in m_scriptable.Characters)
        {
            Selection characterSelection = Instantiate(m_prefab, m_picksField.transform);
            characterSelection.Icon.sprite = character.Character.Icon;
            m_characterSelections.Add(characterSelection);
        }

        m_rowCount = Mathf.FloorToInt((float)m_characterSelections.Count / 4) + 1;

        Pick(Random.Range(0, m_characterSelections.Count), 0);
        Pick(Random.Range(0, m_characterSelections.Count), 1);
    }

    private void Pick(int _characterIndex, int _playerIndex)
    {
        PlayerPick pick = _playerIndex == 0 ? m_playerOnePick : m_playerTwoPick;

        pick.Set(_characterIndex);

        m_characterSelections[_characterIndex].Border.gameObject.SetActive(true);

        if (m_playerOnePick.SelectedCharIndex == m_playerTwoPick.SelectedCharIndex)
        {
            m_characterSelections[_characterIndex].Border.color = m_scriptable.MixedColor;
        }
        else
        {
            m_characterSelections[_characterIndex].Border.color = _playerIndex == 0 ? m_scriptable.PlayerOneColor : m_scriptable.PlayerTwoColor;
        }
    }

    private void UnPick(int _characterIndex)
    {
        if (_characterIndex == m_playerOnePick.SelectedCharIndex)
        {
            m_characterSelections[_characterIndex].Border.color = m_scriptable.PlayerOneColor;
        }
        else if (_characterIndex == m_playerTwoPick.SelectedCharIndex)
        {
            m_characterSelections[_characterIndex].Border.color = m_scriptable.PlayerTwoColor;
        }
        else
        {
            m_characterSelections[_characterIndex].Border.gameObject.SetActive(false);
        }
    }

    private void LockPick(int _playerIndex, bool _lock = true)
    {
        PlayerPick pick = _playerIndex == 0 ? m_playerOnePick : m_playerTwoPick;

        if (pick.SelectedCharIndex == 0)
        {
            int newIndex = Random.Range(1, m_characterSelections.Count);
            Pick(newIndex, _playerIndex);
            UnPick(0);
            Pick(newIndex, _playerIndex);
            LockPick(_playerIndex);
        }
        else
        {
            pick.Lock(_lock);
        }
    }

    public bool OnSelect(int _playerIndex)
    {
        if (m_playerOnePick.IsLocked && m_playerTwoPick.IsLocked)
        {
            gameObject.SetActive(false);
            m_stageSelection.gameObject.SetActive(true);
            return true;
        }
        else
        {
            LockPick(_playerIndex);
            return false;
        }
    }

    public bool OnBack(int _playerIndex)
    {
        PlayerPick pick = _playerIndex == 0 ? m_playerOnePick : m_playerTwoPick;

        if (pick.IsLocked)
        {
            LockPick(_playerIndex, false);
            return false;
        }

        return _playerIndex == 0;
    }

    public void OnWSAD(int _playerIndex, Vector2 _value)
    {
        PlayerPick pick = _playerIndex == 0 ? m_playerOnePick : m_playerTwoPick;

        if (pick.IsLocked) return;

        int selectedIndex = pick.SelectedCharIndex;

        int row = Mathf.FloorToInt((float)selectedIndex / FIELD_WIDTH);
        int column = selectedIndex - (row * FIELD_WIDTH);

        if (_value.x > 0)
        {
            column++;
        }
        else if (_value.x < 0)
        {
            column--;
        }
        else if (_value.y > 0)
        {
            row--;
        }
        else if (_value.y < 0)
        {
            row++;
        }

        column = Mathf.Clamp(column, 0, FIELD_WIDTH - 1);
        row = Mathf.Clamp(row, 0, m_rowCount - 1);

        int newIndex = FIELD_WIDTH * row + column;
        newIndex = Mathf.Clamp(newIndex, 0, m_characterSelections.Count - 1);

        Pick(newIndex, _playerIndex);
        UnPick(selectedIndex);
        Pick(newIndex, _playerIndex);
    }
}
