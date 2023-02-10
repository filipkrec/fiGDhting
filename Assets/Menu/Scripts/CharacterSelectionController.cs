using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour
{
    public const int FIELD_WIDTH = 4;

    [SerializeField] private CharactersScriptable m_scriptable;
    [SerializeField] private MainMenuController m_mainMenuController;

    [SerializeField] private PlayerPick m_playerOnePick;
    [SerializeField] private PlayerPick m_playerTwoPick;

    [SerializeField] private GameObject m_picksField;
    [SerializeField] private Selection m_prefab;
    [SerializeField] private StageSelection m_stageSelection;

    private List<Selection> m_characterSelections = new();
    private int m_rowCount;

    private void Start()
    {
        foreach (CharacterBase character in m_scriptable.Characters)
        {
            Selection characterSelection = Instantiate(m_prefab, m_picksField.transform);
            characterSelection.Icon.sprite = character.Icon;
            m_characterSelections.Add(characterSelection);
        }

        m_rowCount = Mathf.FloorToInt((float)m_characterSelections.Count / 4) + 1;

        Pick(0, 0);
        Pick(0, 1);
    }

    private void Update()
    {
        if (PlayerControllerStatic.IsDown(0, Controls.W)) OnWSAD(0, Directions.Up);
        if (PlayerControllerStatic.IsDown(0, Controls.S)) OnWSAD(0, Directions.Down);
        if (PlayerControllerStatic.IsDown(0, Controls.A)) OnWSAD(0, Directions.Left);
        if (PlayerControllerStatic.IsDown(0, Controls.D)) OnWSAD(0, Directions.Right);

        if (PlayerControllerStatic.IsDown(1, Controls.W)) OnWSAD(1, Directions.Up);
        if (PlayerControllerStatic.IsDown(1, Controls.S)) OnWSAD(1, Directions.Down);
        if (PlayerControllerStatic.IsDown(1, Controls.A)) OnWSAD(1, Directions.Left);
        if (PlayerControllerStatic.IsDown(1, Controls.D)) OnWSAD(1, Directions.Right);

        if (PlayerControllerStatic.IsDown(0, Controls.K1)) OnSelect(0);
        if (PlayerControllerStatic.IsDown(1, Controls.K1)) OnSelect(1);

        if (PlayerControllerStatic.IsDown(0, Controls.K2))
        { 
            if(OnBack(0)) SwitchToMainMenu(); 
        }
        if (PlayerControllerStatic.IsDown(1, Controls.K2))
        {
            OnBack(1);
        }
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

    public void OnSelect(int _playerIndex)
    {
        if (m_playerOnePick.IsLocked && m_playerTwoPick.IsLocked)
        {
            gameObject.SetActive(false);
            m_stageSelection.gameObject.SetActive(true);
        }
        else
        {
            LockPick(_playerIndex);
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

    public void SwitchToMainMenu()
    {
        gameObject.SetActive(false);
        m_mainMenuController.gameObject.SetActive(true);
    }

    public void OnWSAD(int _playerIndex, Directions _direction)
    {
        PlayerPick pick = _playerIndex == 0 ? m_playerOnePick : m_playerTwoPick;

        if (pick.IsLocked) return;

        int selectedIndex = pick.SelectedCharIndex;

        int row = Mathf.FloorToInt((float)selectedIndex / FIELD_WIDTH);
        int column = selectedIndex - (row * FIELD_WIDTH);

        switch(_direction)
        {
            case (Directions.Right):
                column++;
                break;
            case (Directions.Left):
                column--;
                break;
            case (Directions.Down):
                row++;
                break;
            case (Directions.Up):
                row--;
                break;
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
