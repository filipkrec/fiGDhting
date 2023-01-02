using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Menu
{
    MainMenu,
    CharacterSelection
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform m_buttonContainer;
    [SerializeField] private Button m_playButton;
    [SerializeField] private Button m_exitButton;

    [SerializeField] private GameObject m_vsPopup;
    [SerializeField] private CharacterSelectionController m_charSelectionScreen;

    [SerializeField] private TextMeshProUGUI m_p1InputText;
    [SerializeField] private TextMeshProUGUI m_p2InputText;
    [SerializeField] private Menu m_currentMenu;

    private int m_selectedButtonIndex = 0;
    private string m_defaultInputText;
    private List<Button> m_buttons = new();

    private void Awake()
    {
        m_defaultInputText = m_p1InputText.text;

        m_playButton.onClick.AddListener(Play);
        m_exitButton.onClick.AddListener(Exit);

        m_currentMenu = Menu.MainMenu;

        m_playButton.interactable = false;

        foreach (Button btn in m_buttonContainer.GetComponentsInChildren<Button>())
        {
            m_buttons.Add(btn);
        }

        if (Players.s_Players.Count != 0)
        {
            for (int i = 0; i < Players.s_Players.Count; ++i)
            {
                Players.s_Players[i].gameObject.SetActive(true);
                Players.s_Players[i].Init(this, Players.s_Players[i].PlayerIndex);
            }

            PlayerInput p1input = Players.s_Players.Find((x) => x.PlayerIndex == 0).GetComponent<PlayerInput>();
            PlayerInput p2input = Players.s_Players.Find((x) => x.PlayerIndex == 1).GetComponent<PlayerInput>();

            p1input.SwitchCurrentControlScheme(FightSetup.PlayerOne.Device);
            p2input.SwitchCurrentControlScheme(FightSetup.PlayerTwo.Device);
            m_p1InputText.text = $"P1 :\n " + p1input.devices[0].displayName;
            m_p2InputText.text = $"P2 :\n " + p2input.devices[0].displayName;

            m_playButton.interactable = true;
            SelectButton(true);
        }
    }

    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        if (Players.s_Players.Contains(_playerInput.GetComponent<MenuPlayer>())) return;

        if (_playerInput.playerIndex == 0)
        {
            SelectButton(true);
            m_p1InputText.text = $"P1 :\n " + _playerInput.devices[0].displayName;
            FightSetup.PlayerOne.Device = _playerInput.devices[0];
        }
        else
        {
            m_p2InputText.text = $"P2 :\n " + _playerInput.devices[0].displayName;
            FightSetup.PlayerTwo.Device = _playerInput.devices[0];
        }

        MenuPlayer player = _playerInput.GetComponent<MenuPlayer>();
        player.Init(this, _playerInput.playerIndex);

        Players.s_Players.Add(player);

        if (Players.s_Players.Count == 2)
        {
            m_playButton.interactable = true;
        }
    }

    private void SelectButton(bool _next)
    {
        m_selectedButtonIndex += _next ? 1 : -1;
        m_selectedButtonIndex = Mathf.Clamp(m_selectedButtonIndex, 0, m_buttons.Count - 1);

        if (!m_buttons[m_selectedButtonIndex].interactable)
        {
            bool isLast = m_selectedButtonIndex == m_buttons.Count - 1;
            bool isFirst = m_selectedButtonIndex == 0;
            if (isFirst)
            {
                SelectButton(true);
            }
            else if (isLast)
            {
                SelectButton(false);
            }
            else
            {
                SelectButton(_next);
            }
        }
        else
        {
            m_buttons[m_selectedButtonIndex].Select();
        }
    }

    private void Play()
    {
        if (m_currentMenu != Menu.MainMenu) return;

        m_currentMenu = Menu.CharacterSelection;
        m_vsPopup.SetActive(true);//Plays the animation with event to switch CharSelection to active
    }

    private void Exit()
    {
        if (m_currentMenu != Menu.MainMenu) return;

        Application.Quit();
    }

    public void OnSelect(int _playerIndex)
    {
        if (m_currentMenu == Menu.MainMenu)
        {
            if (!CheckPrimaryPlayer(_playerIndex)) return;
            m_buttons[m_selectedButtonIndex].onClick?.Invoke();
        }
        else if (m_currentMenu == Menu.CharacterSelection)
        {
            m_charSelectionScreen.OnSelect(_playerIndex);
        }
    }

    public void OnWSAD(int _playerIndex, Vector2 _value)
    {
        if (m_currentMenu == Menu.MainMenu)
        {
            if (_playerIndex != 0) return;
            if (_value.y < 0)
            {
                SelectButton(true);
            }
            else if (_value.y > 0)
            {
                SelectButton(false);
            }
        }
        else if (m_currentMenu == Menu.CharacterSelection)
        {
            m_charSelectionScreen.OnWSAD(_playerIndex, _value);
        }
    }

    public void OnBack(int _playerIndex)
    {
        if (m_currentMenu == Menu.MainMenu)
        {
            MenuPlayer player = Players.s_Players.Find((x) => x.PlayerIndex == _playerIndex);
            Players.s_Players.Remove(player);

            Destroy(player.gameObject);

            m_playButton.interactable = false;

            if (_playerIndex == 0)
            {
                m_p1InputText.text = m_defaultInputText;
            }
            else if (_playerIndex == 1)
            {
                m_p2InputText.text = m_defaultInputText;
            }
        }
        else if (m_currentMenu == Menu.CharacterSelection)
        {
            if (m_charSelectionScreen.gameObject.activeInHierarchy)
            {
                bool goBack = m_charSelectionScreen.OnBack(_playerIndex);

                if (goBack)
                {
                    m_selectedButtonIndex = -1;
                    SelectButton(true);
                    m_charSelectionScreen.gameObject.SetActive(false);
                    m_currentMenu = Menu.MainMenu;
                }
            }
        }
    }

    private bool CheckPrimaryPlayer(int _playerIndex)
    {
        if (Players.s_Players.Count == 2)
        {
            return _playerIndex == 0;
        }
        else return true;
    }
}
