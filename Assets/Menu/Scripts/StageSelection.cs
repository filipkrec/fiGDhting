using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelection : MonoBehaviour
{
    [SerializeField] private CharactersScriptable m_charScriptable;
    [SerializeField] private StagesScriptable m_scriptable;

    [SerializeField] private Selection m_prefab;
    [SerializeField] private GameObject m_picksField;

    private int p1pick;
    private int p2pick;
    private bool p1Locked;
    private bool p2Locked;

    private List<Selection> m_characterSelections = new();
    private int m_rowCount;

    private void Start()
    {
        foreach (Sprite sprite in m_scriptable.stages)
        {
            Selection stageSelection = Instantiate(m_prefab, m_picksField.transform);
            stageSelection.Icon.sprite = sprite;
            m_characterSelections.Add(stageSelection);
        }

        m_rowCount = Mathf.FloorToInt((float)m_characterSelections.Count / 4) + 1;

        Pick(Random.Range(0, m_characterSelections.Count), 0);
        Pick(Random.Range(0, m_characterSelections.Count), 1);
    }

    private void Pick(int _characterIndex, int _playerIndex)
    {
        if (_playerIndex == 0)
        {
            p1pick = _characterIndex;
        }
        else
        {
            p2pick = _characterIndex;
        }

        m_characterSelections[_characterIndex].Border.gameObject.SetActive(true);

        if (p1pick == p2pick)
        {
            m_characterSelections[_characterIndex].Border.color = m_charScriptable.MixedColor;
        }
        else
        {
            m_characterSelections[_characterIndex].Border.color = _playerIndex == 0 ? m_charScriptable.PlayerOneColor : m_charScriptable.PlayerTwoColor;
        }
    }

    private void UnPick(int _characterIndex)
    {
        if (_characterIndex == p1pick)
        {
            m_characterSelections[_characterIndex].Border.color = m_charScriptable.PlayerOneColor;
        }
        else if (_characterIndex == p2pick)
        {
            m_characterSelections[_characterIndex].Border.color = m_charScriptable.PlayerTwoColor;
        }
        else
        {
            m_characterSelections[_characterIndex].Border.gameObject.SetActive(false);
        }
    }

    private void LockPick(int _playerIndex, bool _lock = true)
    {
        int pick = _playerIndex == 0 ? p1pick : p2pick;

        if (pick == 0)
        {
            int newIndex = Random.Range(1, m_characterSelections.Count);
            Pick(newIndex, _playerIndex);
            UnPick(0);
            Pick(newIndex, _playerIndex);
            LockPick(_playerIndex);
        }
        else
        {
            Lock(_playerIndex, pick, _lock);
        }
    }

    public void OnSelect(int _playerIndex)
    {
        if (p1Locked && p2Locked)
        {
            foreach (MenuPlayer player in Players.s_Players)
            {
                player.gameObject.SetActive(false);
            }

            SceneManager.LoadScene(1);
        }
        else
        {
            LockPick(_playerIndex);
        }
    }

    public bool OnBack(int _playerIndex)
    {
        int pick = _playerIndex == 0 ? p1pick : p2pick;

        if ((p1Locked && _playerIndex == 0) || (p2Locked && _playerIndex == 1))
        {
            LockPick(_playerIndex, false);
            return false;
        }

        return _playerIndex == 0;
    }

    public void OnWSAD(int _playerIndex, Vector2 _value)
    {

        if ((p1Locked && _playerIndex == 0) || (p2Locked && _playerIndex == 1)) return;

        int selectedIndex = _playerIndex == 0 ? p1pick : p2pick;

        int row = Mathf.FloorToInt((float)selectedIndex / CharacterSelectionController.FIELD_WIDTH);
        int column = selectedIndex - (row * CharacterSelectionController.FIELD_WIDTH);

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

        column = Mathf.Clamp(column, 0, CharacterSelectionController.FIELD_WIDTH - 1);
        row = Mathf.Clamp(row, 0, m_rowCount - 1);

        int newIndex = CharacterSelectionController.FIELD_WIDTH * row + column;
        newIndex = Mathf.Clamp(newIndex, 0, m_characterSelections.Count - 1);

        Pick(newIndex, _playerIndex);
        UnPick(selectedIndex);
        Pick(newIndex, _playerIndex);
    }

    private void Lock(int _playerIndex, int _pick, bool _true)
    {
        if(_playerIndex == 0)
        {
            p1pick = _pick;
            p1Locked = _true;
        }
        else if (_playerIndex == 1)
        {
            p2pick = _pick;
            p2Locked = _true;
        }
    }
}
