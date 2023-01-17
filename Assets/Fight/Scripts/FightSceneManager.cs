using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightSceneManager : MonoBehaviour
{
    private static CharacterBase PlayerOne;
    private static CharacterBase PlayerTwo;

    [SerializeField] private Rounds m_rounds;
    [SerializeField] private Pause m_pause;
    [SerializeField] private Image m_stage; 

    [SerializeField] private HealthBar m_leftHPBar;
    [SerializeField] private HealthBar m_rightHPBar;

    [SerializeField] private Vector2 m_playerSpawn;
    [SerializeField] private Transform m_leftBorder;
    [SerializeField] private Transform m_rightBorder;

    [SerializeField] private CharactersScriptable scriptable;

    private void Awake()
    {
        if(FightSetup.PlayerOne.Character == null && FightSetup.PlayerTwo.Character == null)
        {
            FightSetup.PlayerOne.Character = scriptable.Characters[5];
            FightSetup.PlayerTwo.Character = scriptable.Characters[6];
        }

        m_stage.sprite = FightSetup.SelectedStage;

        PlayerOne = Instantiate(FightSetup.PlayerOne.Character);
        PlayerTwo = Instantiate(FightSetup.PlayerTwo.Character);

        PlayerTwo.transform.position = m_playerSpawn;

        Vector2 p1Spawn = m_playerSpawn;
        p1Spawn.x = -p1Spawn.x;
        PlayerOne.transform.position = p1Spawn;
        PlayerOne.FaceRight(true);

        PlayerOne.Setup(m_leftHPBar, this);
        PlayerTwo.Setup(m_rightHPBar, this);

        PlayerInput playerInput = PlayerOne.GetComponent<PlayerInput>();
        if (FightSetup.PlayerOne.Device != null)
        {
            playerInput.SwitchCurrentControlScheme(FightSetup.PlayerOne.Device);
        }

        playerInput = PlayerTwo.GetComponent<PlayerInput>();
        if (FightSetup.PlayerTwo.Device != null)
        {
            playerInput.SwitchCurrentControlScheme(FightSetup.PlayerTwo.Device);
        }
    }

    public void CheckRotations()
    {
        if (PlayerOne.FacingRight && PlayerOne.transform.position.x > PlayerTwo.transform.position.x)
        {
            PlayerOne.FaceRight(false);
            PlayerTwo.FaceRight(true);
        }
        else if (!PlayerOne.FacingRight && PlayerOne.transform.position.x < PlayerTwo.transform.position.x)
        {
            PlayerOne.FaceRight(true);
            PlayerTwo.FaceRight(false);
        }
    }

    public void CheckBorders(Transform _trans)
    {
        if(_trans.position.x > m_rightBorder.position.x)
        {
            _trans.position = new Vector2(m_rightBorder.position.x, _trans.position.y);
        }
        else if(_trans.position.x < m_leftBorder.position.x)
        {
            _trans.position = new Vector2(m_leftBorder.position.x, _trans.position.y);
        }
    }

    public void RestartFight()
    {
        SceneManager.LoadScene(1);
    }

    public void WinRound(CharacterBase _losingCharacter)
    {
        int _winner = 0;
        if (PlayerOne == _losingCharacter) _winner = 2;
        else _winner = 1;

        m_rounds.gameObject.SetActive(true);
        m_rounds.SetWinner(_winner, _winner == 1 ? PlayerOne.Name : PlayerTwo.Name);
    }

    public void Win(int _player)
    {
        m_pause.SetWinner(_player);
        m_pause.gameObject.SetActive(true);
    }

    public void MenuDown()
    {
        if(m_pause.isActiveAndEnabled)
        {
            m_pause.OnDown();
        }
    }

    public void MenuUp()
    {
        if (m_pause.isActiveAndEnabled)
        {
            m_pause.OnUp();
        }
    }

    public void MenuSelect()
    {
        if (m_pause.isActiveAndEnabled)
        {
            m_pause.OnSelect();
        }
    }

    public void MenuPause()
    {
        if(!m_pause.Finished && m_pause.gameObject.activeInHierarchy)
        {
            m_pause.gameObject.SetActive(false);
        }
        else if (!m_pause.gameObject.activeInHierarchy)
        {
            m_pause.gameObject.SetActive(true);
        }
    }
}
