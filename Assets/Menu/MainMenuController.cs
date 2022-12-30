using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button m_playButton;
    [SerializeField] private Button m_exitButton;

    [SerializeField] private GameObject m_vsPopup;

    [SerializeField] private TextMeshProUGUI m_p1InputText;
    [SerializeField] private TextMeshProUGUI m_p2InputText;

    private void Awake()
    {
        m_playButton.onClick.AddListener(Play);
        m_exitButton.onClick.AddListener(Exit);

        m_playButton.interactable = false;
    }

    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        if (_playerInput.playerIndex > 2)
        {

        }
        else
        {
            if(_playerInput.playerIndex == 0)
            {
                FightSetup.PlayerOne.Device = _playerInput.devices[0];
                m_p1InputText.text = "P1 :\n " + _playerInput.devices[0].displayName;
            }
            else
            {
                FightSetup.PlayerTwo.Device = _playerInput.devices[0];
                m_p2InputText.text = "P2 :\n " + _playerInput.devices[0].displayName;

                m_playButton.interactable = true;
            }
        }
    }

    private void Play()
    {
        //SceneManager.LoadScene(1);
        m_vsPopup.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
