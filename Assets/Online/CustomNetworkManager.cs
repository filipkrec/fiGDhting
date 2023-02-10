using Mirror;
using System;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static bool IsHost = NetworkServer.active && NetworkClient.active;
    public static bool ClientConnected = NetworkClient.active;

    private static int m_playerCount = 0;

    public static Action OnPlayersConnected;

    public static int OnPlayerConnect()
    {
        int playerCount = m_playerCount;

        m_playerCount++;

        Debug.Log(m_playerCount);

        if (m_playerCount == 2)
        {
            OnPlayersConnected?.Invoke();
        }

        return playerCount;
    }

    public static void OnPlayerDisconnect()
    {
        m_playerCount--;
    }
}
