using System.Collections.Generic;

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


public static class PlayerControls
{
    private static Dictionary<int, List<Controls>> m_playerActions;

    public static void OnAction(int _playerIndex, Controls _control, bool _used)
    {
        if (!m_playerActions.ContainsKey(_playerIndex))
        {
            m_playerActions.Add(_playerIndex, new List<Controls>());
        }

        if (_used)
        {
            m_playerActions[_playerIndex].Add(_control);
        }
        else
        {
            m_playerActions[_playerIndex].Remove(_control);
        }
    }

    public static bool IsUsed(int _playerIndex, Controls _control)
    {
        if (!m_playerActions.ContainsKey(_playerIndex)) return false;

        return m_playerActions[_playerIndex].Contains(_control);
    }
}
