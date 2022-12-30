using UnityEngine;
using UnityEngine.InputSystem;

public class FightSceneManager : MonoBehaviour
{
    private static CharacterBase PlayerOne;
    private static CharacterBase PlayerTwo;

    [SerializeField] private HealthBar m_leftHPBar;
    [SerializeField] private HealthBar m_rightHPBar;

    [SerializeField] private Vector2 m_playerSpawn;

    [SerializeField] private CharactersScriptable scriptable;

    private void Awake()
    {
        FightSetup.PlayerOne.Character = scriptable.Characters[0];
        FightSetup.PlayerTwo.Character = scriptable.Characters[0];

        PlayerOne = Instantiate(FightSetup.PlayerOne.Character);
        PlayerTwo = Instantiate(FightSetup.PlayerTwo.Character);

        PlayerTwo.transform.position = m_playerSpawn;

        m_playerSpawn.x = -m_playerSpawn.x;
        PlayerOne.transform.position = m_playerSpawn;
        PlayerOne.FaceRight(true);

        PlayerOne.Setup(m_leftHPBar, this);
        PlayerTwo.Setup(m_rightHPBar, this);

        PlayerInput playerInput = PlayerOne.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(FightSetup.PlayerOne.Device);

        playerInput = PlayerTwo.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(FightSetup.PlayerTwo.Device);
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
}
