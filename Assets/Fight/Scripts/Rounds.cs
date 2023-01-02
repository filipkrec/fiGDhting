using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Rounds : MonoBehaviour
{
    private const string m_winText = "![P]!\n!Wins!";
    [SerializeField] private GameObject m_p1Round;
    [SerializeField] private GameObject m_p2Round;
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private FightSceneManager m_manager;

    int m_winner;

    public void SetWinner(int _player, string _characterName)
    {
        m_winner = _player;
        m_text.text = Regex.Replace(m_winText, "[[]P[]]", _characterName);
    }

    public void AnimationOver()
    {
        GameObject winnerRound = m_winner == 1 ? m_p1Round : m_p2Round;

        if (winnerRound.activeInHierarchy)
        {
            m_manager.Win(m_winner);
        }
        else
        {
            winnerRound.SetActive(true);
            m_manager.RestartFight();
        }

        gameObject.SetActive(false);
    }
}
