using Mirror;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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
    [Command] public void DoW(bool _pressed) { P1 = _pressed; }
    [Command] public void DoS(bool _pressed) { P2 = _pressed; }
    [Command] public void DoA(bool _pressed) { K1 = _pressed; }
    [Command] public void DoD(bool _pressed) { K2 = _pressed; }
    [Command] public void DoJoin(bool _pressed) { Join = _pressed; }
    [Command] public void DoJ(bool _pressed) { K2 = _pressed; }

    public void OnP1(bool _old, bool _new) { Debug.Log("P1"); }
    public void OnP2(bool _old, bool _new) { Debug.Log("P2"); }
    public void OnK1(bool _old, bool _new) { Debug.Log("K1"); }
    public void OnK2(bool _old, bool _new) { Debug.Log("K2"); }
    public void OnW(bool _old, bool _new) { Debug.Log("W"); }
    public void OnS(bool _old, bool _new) { Debug.Log("S"); }
    public void OnA(bool _old, bool _new) { Debug.Log("A"); }
    public void OnD(bool _old, bool _new) { Debug.Log("D"); }
    public void OnJoin(bool _old, bool _new) { Debug.Log("Join"); }
    public void OnJ(bool _old, bool _new) { Debug.Log("J"); }

    private void Start()
    {
        DontDestroyOnLoad(this);
        m_playerId = CustomNetworkManager.OnPlayerConnect();
        m_isLocal = isLocalPlayer;

        if(isLocalPlayer)
        {
            MenuPlayer player = FindObjectsOfType<MenuPlayer>()[0];
            player.NetworkPlayer = this;
        }
    }

    private void OnDestroy()
    {
        CustomNetworkManager.OnPlayerDisconnect();
    }

    public void StartInput(CallbackContext _context)
    {
        if(isLocalPlayer)
        {
            if (_context.started == true)
            {
                DoJoin(true);
            }
            else if (_context.canceled == true)
            {
                DoJoin(false);
            }
        }
    }
}
