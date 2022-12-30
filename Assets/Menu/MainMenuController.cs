using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button m_playButton;
    [SerializeField] private Button m_exitButton;

    [SerializeField] private GameObject m_vsPopup;

    private void Awake()
    {
        m_playButton.onClick.AddListener(Play);
        m_exitButton.onClick.AddListener(Exit);
    }

    private void Play()
    {
        m_vsPopup.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
