using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput; 

    public NetworkPlayer NetworkPlayer = null;

    private MainMenuController m_mainMenuController;

    private int m_playerIndex;

    public int PlayerIndex => m_playerIndex;

    private void Start()
    {
        DontDestroyOnLoad(this);

        FindObjectOfType<MainMenuController>().OnPlayerJoined(m_playerInput);

        m_playerIndex = m_playerInput.playerIndex;
    }

    private bool? ButtonPressed(CallbackContext _context)
    {
        if (_context.started) return true;
        else if (_context.canceled) return false;
        else return null;
    }

    public void P1(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoP1((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.P1, (bool)ButtonPressed(_context));
    }

    public void P2(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoP2((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.P2, (bool)ButtonPressed(_context));
    }

    public void K1(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoK1((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.K1, (bool)ButtonPressed(_context));
    }

    public void K2(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoK2((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.K2, (bool)ButtonPressed(_context));
    }

    public void W(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoW((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.W, (bool)ButtonPressed(_context));
    }

    public void S(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoS((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.S, (bool)ButtonPressed(_context));
    }

    public void A(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoA((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.A, (bool)ButtonPressed(_context));
    }

    public void D(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoD((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.D, (bool)ButtonPressed(_context));
    }

    public void Join(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoJoin((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.Join, (bool)ButtonPressed(_context));
    }

    public void J(CallbackContext _context) 
    {
        if (ButtonPressed(_context) == null) return;

        if (NetworkPlayer != null) NetworkPlayer.DoJ((bool)ButtonPressed(_context));
        else PlayerControllerStatic.OnAction(m_playerIndex, Controls.J, (bool)ButtonPressed(_context));
    }
}
