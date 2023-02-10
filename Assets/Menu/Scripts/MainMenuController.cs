using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Menu
{
    MainMenu,
    CharacterSelection,
    StageSelection,
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform m_buttonContainer;
    [SerializeField] private Button m_playButton;
    [SerializeField] private Button m_onlineButton;
    [SerializeField] private Button m_exitButton;

    [SerializeField] private GameObject m_vsPopup;
    [SerializeField] private OnlineScreen m_onlineScreen;
    [SerializeField] private CharacterSelectionController m_charSelectionScreen;
    [SerializeField] private StageSelection m_stageSelectionScreen;

    [SerializeField] private TextMeshProUGUI m_p1InputText;
    [SerializeField] private TextMeshProUGUI m_p2InputText;

    private int m_selectedButtonIndex = 0;
    private string m_defaultInputText;
    private List<Button> m_buttons = new();

    private void Awake()
    {
        m_defaultInputText = m_p1InputText.text;

        m_playButton.onClick.AddListener(Play);
        m_exitButton.onClick.AddListener(Exit);
        m_onlineButton.onClick.AddListener(() => m_onlineScreen.gameObject.SetActive(true));

        m_playButton.interactable = false;
        m_onlineButton.interactable = false;

        foreach (Button btn in m_buttonContainer.GetComponentsInChildren<Button>())
        {
            m_buttons.Add(btn);
        }
    }

    private void Update()
    {
        if (PlayerControllerStatic.IsDown(0, Controls.K1))
        {
            OnSelect();
        }

        if (PlayerControllerStatic.IsDown(0, Controls.K2))
        {
            OnBack(0);
        }

        if (PlayerControllerStatic.IsDown(1, Controls.K2))
        {
            OnBack(1);
        }

        if (PlayerControllerStatic.IsDown(0, Controls.W))
        {
            SelectButton(false);
        }

        if (PlayerControllerStatic.IsDown(0, Controls.S))
        {
            SelectButton(true);
        }
    }

    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        if (Players.s_Players.Contains(_playerInput.GetComponent<InputPlayer>())) return;

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

        InputPlayer player = _playerInput.GetComponent<InputPlayer>();

        Players.s_Players.Add(player);

        if (Players.s_Players.Count == 2)
        {
            m_onlineButton.interactable = false;
            m_playButton.interactable = true;
        }
        else if (Players.s_Players.Count == 1)
        {
            m_onlineButton.interactable = true;
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

    public void Play()
    {
        m_vsPopup.SetActive(true); //Plays the animation with event to switch to CharSelection
    }

    private void Exit()
    {
        Application.Quit();
    }

    public void OnSelect()
    {
        m_buttons[m_selectedButtonIndex].onClick?.Invoke();
    }

    public void OnWSAD(int _playerIndex, Vector2 _value)
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

    public void OnBack(int _playerIndex)
    {
        InputPlayer player = Players.s_Players.Find((x) => x.PlayerIndex == _playerIndex);
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
}
