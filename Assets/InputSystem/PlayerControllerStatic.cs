using System.Collections.Generic;
using UnityEngine;

public enum Controls
{
    P1,
    P2,
    K1,
    K2,
    Join,
    J,
    W,
    S,
    A,
    D
}


public class PlayerControllerStatic : MonoBehaviour
{
    private static Dictionary<int, List<Controls>> m_pressed = new ();
    private static Dictionary<int, List<Controls>> m_buttonDown = new();
    private static Dictionary<int, List<Controls>> m_buttonUp = new();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
    }

    private void LateUpdate()
    {
        foreach (var key in m_buttonDown.Keys)
        {
            m_buttonDown[key].Clear();
        }
        foreach (var key in m_buttonUp.Keys)
        {
            m_buttonUp[key].Clear();
        }
    }

    public static void OnAction(int _playerIndex, Controls _control, bool _used)
    {
        if (!m_pressed.ContainsKey(_playerIndex))
        {
            m_pressed.Add(_playerIndex, new List<Controls>());
            m_buttonDown.Add(_playerIndex, new List<Controls>());
            m_buttonUp.Add(_playerIndex, new List<Controls>());
        }

        if (_used)
        {
            m_pressed[_playerIndex].Add(_control);
            m_buttonDown[_playerIndex].Add(_control);
        }
        else
        {
            m_pressed[_playerIndex].Remove(_control);
            m_buttonUp[_playerIndex].Add(_control);
        }
    }

    public static bool IsPressed(int _playerIndex, Controls _control)
    {
        if (!m_pressed.ContainsKey(_playerIndex)) return false;

        return m_pressed[_playerIndex].Contains(_control);
    }

    public static bool IsDown(int _playerIndex, Controls _control)
    {
        if (!m_buttonDown.ContainsKey(_playerIndex)) return false;

        return m_buttonDown[_playerIndex].Contains(_control);
    }

    public static bool IsUp(int _playerIndex, Controls _control)
    {
        if (!m_buttonUp.ContainsKey(_playerIndex)) return false;

        return m_buttonUp[_playerIndex].Contains(_control);
    }
}
