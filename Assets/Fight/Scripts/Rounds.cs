using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Rounds : MonoBehaviour
{
    private const string m_winText = "![P]!\n!Wins!";
    private static int p1wins = 0;
    private static int p2wins = 0;

    [SerializeField] private GameObject m_p1Round;
    [SerializeField] private GameObject m_p2Round;
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private FightSceneManager m_manager;

    int m_winner;

    private void Start()
    {
        if(p1wins == 1)
        {
            m_p1Round.SetActive(true);
        }
        else if(p2wins == 1)
        {
            m_p1Round.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    public void SetWinner(int _player, string _characterName)
    {
        m_winner = _player;
        m_text.text = Regex.Replace(m_winText, "[[]P[]]", _characterName);
    }

    public void AnimationOver()
    {
        if(m_winner == 1) p1wins++;
        else p2wins++;

        if (p1wins == 2 || p2wins == 2)
        {
            m_manager.Win(m_winner);
        }
        else
        {
            m_manager.RestartFight();
        }

        gameObject.SetActive(false);
    }
}
