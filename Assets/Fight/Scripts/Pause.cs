using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class Pause : MonoBehaviour
{
    public static bool Paused => m_paused;
    private static bool m_paused;

    private const string WIN_TEXT = "[P] Victory";

    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Button m_restartButton; 
    [SerializeField] private Button m_backButton;

    private Button m_selectedButton;

    private bool m_finished;
    public bool Finished => m_finished;

    private void Start()
    {
        m_backButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        m_restartButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    private void OnEnable()
    {
        m_paused = true;
        Time.timeScale = 0;
        OnUp();
    }

    private void OnDisable()
    {
        m_paused = false;
        Time.timeScale = 1;
    }

    public void SetWinner(int _player)
    {
        m_text.gameObject.SetActive(true);
        m_text.text = Regex.Replace(WIN_TEXT,"[[]P[]]", "P" + _player);
    }

    internal void OnDown()
    {
        m_backButton.Select();
        m_selectedButton = m_backButton;
    }

    internal void OnUp()
    {
        m_restartButton.Select();
        m_selectedButton = m_restartButton;
    }

    internal void OnSelect()
    {
        m_selectedButton.onClick?.Invoke();
    }
}
