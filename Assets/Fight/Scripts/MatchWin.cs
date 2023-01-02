using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MatchWin : MonoBehaviour
{
    private const string WIN_TEXT = "[P] Victory";

    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Button m_restartButton; 
    [SerializeField] private Button m_backButton;

    private void Start()
    {
        m_backButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        m_restartButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    public void SetWinner(int _player)
    {
        m_text.text = Regex.Replace(WIN_TEXT,"[P]", "P" + _player);
    }
}
