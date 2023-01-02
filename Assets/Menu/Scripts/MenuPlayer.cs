using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class MenuPlayer : MonoBehaviour
{
    MainMenuController m_controller;
    private int m_playerIndex;

    public int PlayerIndex => m_playerIndex;

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Init(MainMenuController _controller, int _playerIndex)
    {
        m_playerIndex = _playerIndex;
        m_controller = _controller;
    }

    public void OnSelect(CallbackContext _context)
    {
        if (m_controller == null || _context.started != true) return;

        m_controller.OnSelect(m_playerIndex);
    }

    public void OnWSAD(CallbackContext _context)
    {
        if (m_controller == null || _context.started != true) return;

        Vector2 value = _context.ReadValue<Vector2>();
        m_controller.OnWSAD(m_playerIndex, value);
    }

    public void OnBack(CallbackContext _context)
    {
        if (m_controller == null || _context.started != true) return;

        m_controller.OnBack(m_playerIndex);
    }
}
