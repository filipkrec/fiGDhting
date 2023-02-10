using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private int m_playerId = 0;
    [SerializeField] private bool m_isLocal = false;

    [SyncVar(hook = nameof(OnP1))] public bool P1 = false;
    [SyncVar(hook = nameof(OnP2))] public bool P2 = false;
    [SyncVar(hook = nameof(OnK1))] public bool K1 = false;
    [SyncVar(hook = nameof(OnK2))] public bool K2 = false;
    [SyncVar(hook = nameof(OnW))] public bool W = false;
    [SyncVar(hook = nameof(OnS))] public bool S = false;
    [SyncVar(hook = nameof(OnA))] public bool A = false;
    [SyncVar(hook = nameof(OnD))] public bool D = false;
    [SyncVar(hook = nameof(OnJoin))] public bool Join = false;
    [SyncVar(hook = nameof(OnJ))] public bool J = false;

    [Command] public void DoP1(bool _pressed) { P1 = _pressed; }
    [Command] public void DoP2(bool _pressed) { P2 = _pressed; }
    [Command] public void DoK1(bool _pressed) { K1 = _pressed; }
    [Command] public void DoK2(bool _pressed) { K2 = _pressed; }
    [Command] public void DoW(bool _pressed) { W = _pressed; }
    [Command] public void DoS(bool _pressed) { S = _pressed; }
    [Command] public void DoA(bool _pressed) { A = _pressed; }
    [Command] public void DoD(bool _pressed) { D = _pressed; }
    [Command] public void DoJoin(bool _pressed) { Join = _pressed; }
    [Command] public void DoJ(bool _pressed) { J = _pressed; }

    public void OnP1(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.P1, _new); }
    public void OnP2(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.P2, _new); }
    public void OnK1(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.K1, _new); }
    public void OnK2(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.K2, _new); }
    public void OnW(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.W, _new); }
    public void OnS(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.S, _new); }
    public void OnA(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.A, _new); }
    public void OnD(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.D, _new); }
    public void OnJoin(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.Join, _new); }
    public void OnJ(bool _old, bool _new) { PlayerControllerStatic.OnAction(m_playerId, Controls.J, _new); }

    private void Start()
    {
        DontDestroyOnLoad(this);
        m_playerId = CustomNetworkManager.OnPlayerConnect();
        m_isLocal = isLocalPlayer;

        if (isLocalPlayer)
        {
            InputPlayer player = FindObjectsOfType<InputPlayer>()[0];
            player.NetworkPlayer = this;
        }
        syncDirection = isLocalPlayer ? SyncDirection.ServerToClient : SyncDirection.ClientToServer;
        if(syncDirection == SyncDirection.ServerToClient && CustomNetworkManager.IsHost)
        {
            syncMode = SyncMode.Owner;
        }
    }

    private void OnDestroy()
    {
        CustomNetworkManager.OnPlayerDisconnect();
    }
}
