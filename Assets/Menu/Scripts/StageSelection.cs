using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    [SerializeField] private CharactersScriptable m_charScriptable;
    [SerializeField] private CharacterSelectionController m_characterSelectionContr;
    [SerializeField] private StagesScriptable m_scriptable;

    [SerializeField] private Selection m_prefab;
    [SerializeField] private GameObject m_picksField;

    [SerializeField] private PlayerPick m_playerOnePick;
    [SerializeField] private PlayerPick m_playerTwoPick;

    [SerializeField] private Image m_selectedStage;

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
            if (OnBack(0)) SwitchToCharacterSelection();
        }
        if (PlayerControllerStatic.IsDown(1, Controls.K2))
        {
            OnBack(1);
        }
    }

    private void Pick(int _characterIndex, int _playerIndex)
    {
        if (_playerIndex == 0)
        {
            m_playerOnePick.SetStage(_characterIndex);
        }
        else
        {
            m_playerTwoPick.SetStage(_characterIndex);
        }

        m_characterSelections[_characterIndex].Border.gameObject.SetActive(true);

        if (m_playerOnePick.SelectedCharIndex == m_playerTwoPick.SelectedCharIndex)
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
        if (_characterIndex == m_playerOnePick.SelectedCharIndex)
        {
            m_characterSelections[_characterIndex].Border.color = m_charScriptable.PlayerOneColor;
        }
        else if (_characterIndex == m_playerTwoPick.SelectedCharIndex)
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
        int pick = _playerIndex == 0 ? m_playerOnePick.SelectedCharIndex : m_playerTwoPick.SelectedCharIndex;

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
            Lock(_playerIndex, _lock);
        }
    }

    public void OnSelect(int _playerIndex)
    {
        if (m_playerOnePick.IsLocked && m_playerTwoPick.IsLocked)
        {
            StartCoroutine(StartGameCoroutine());
        }
        else
        {
            LockPick(_playerIndex);
        }
    }

    public bool OnBack(int _playerIndex)
    {
        int pick = _playerIndex == 0 ? m_playerOnePick.SelectedCharIndex : m_playerTwoPick.SelectedCharIndex;

        if ((m_playerOnePick.IsLocked && _playerIndex == 0) || (m_playerTwoPick.IsLocked && _playerIndex == 1))
        {
            LockPick(_playerIndex, false);
            return false;
        }

        return _playerIndex == 0;
    }

    private void SwitchToCharacterSelection()
    {
        gameObject.SetActive(false);
        m_characterSelectionContr.gameObject.SetActive(true);
    }

    public void OnWSAD(int _playerIndex, Directions _direction)
    {
        if ((m_playerOnePick.IsLocked && _playerIndex == 0) || (m_playerTwoPick.IsLocked && _playerIndex == 1)) return;

        int selectedIndex = _playerIndex == 0 ? m_playerOnePick.SelectedCharIndex : m_playerTwoPick.SelectedCharIndex;

        int row = Mathf.FloorToInt((float)selectedIndex / CharacterSelectionController.FIELD_WIDTH);
        int column = selectedIndex - (row * CharacterSelectionController.FIELD_WIDTH);

        switch (_direction)
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

        column = Mathf.Clamp(column, 0, CharacterSelectionController.FIELD_WIDTH - 1);
        row = Mathf.Clamp(row, 0, m_rowCount - 1);

        int newIndex = CharacterSelectionController.FIELD_WIDTH * row + column;
        newIndex = Mathf.Clamp(newIndex, 0, m_characterSelections.Count - 1);

        Pick(newIndex, _playerIndex);
        UnPick(selectedIndex);
        Pick(newIndex, _playerIndex);
    }

    private void Lock(int _playerIndex, bool _true)
    {
        if (_playerIndex == 0)
        {
            m_playerOnePick.Lock(_true, true);
        }
        else if (_playerIndex == 1)
        {
            m_playerTwoPick.Lock(_true, true);
        }
    }

    private IEnumerator StartGameCoroutine()
    {
        m_selectedStage.gameObject.SetActive(true);

        int selected = 0;

        if (m_playerOnePick.SelectedCharIndex == m_playerTwoPick.SelectedCharIndex)
        {
            m_selectedStage.sprite = m_playerOnePick.m_image.sprite;
        }
        else
        {
            float time = Random.Range(1f, 3f);
            float step = 0.1f;

            while (time > 0)
            {

                selected = SwapSelected(selected);
                time -= step;
                yield return new WaitForSeconds(step);
            }

            time = 2f;
            while (time > 0f)
            {
                selected = SwapSelected(selected);
                float progressiveStep = Mathf.Lerp(2.1f - time, 2f, 0.5f);
                time -= progressiveStep;
                yield return new WaitForSeconds(progressiveStep);
            }
        }

        selected = SwapSelected(selected);
        yield return new WaitForSeconds(2f);

        FightSetup.SelectedStage = selected ==
            0
            ? m_scriptable.stages[m_playerOnePick.SelectedCharIndex]
            : m_scriptable.stages[m_playerTwoPick.SelectedCharIndex];

        SceneManager.LoadScene(1);
    }

    private int SwapSelected(int current)
    {
        int selected = current == 0 ? 1 : 0;

        if (selected == 0)
        {
            m_selectedStage.sprite = m_playerOnePick.m_image.sprite;
        }
        else if (selected == 1)
        {
            m_selectedStage.sprite = m_playerTwoPick.m_image.sprite;
        }

        return selected;
    }
}
