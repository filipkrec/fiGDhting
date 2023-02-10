using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnlineScreen : MonoBehaviour
{
    [SerializeField] private MainMenuController m_mainMenuController;

    [SerializeField] private CustomNetworkManager manager;

    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_clientScreen;
    [SerializeField] private GameObject m_hostScreen;

    [SerializeField] private Button m_backButton;
    [SerializeField] private Button m_hostButton;
    [SerializeField] private Button m_joinButton;

    //CLIENT
    [SerializeField] private TMP_InputField m_inputField;
    [SerializeField] private Button m_clientJoinBtn;
    [SerializeField] private Button m_clientBackBtn;

    //Host 
    [SerializeField] private Button m_hostCancelBtn;
    [SerializeField] private GameObject m_spinner;

    private bool m_waitingForNetwork = false;

    private void Start()
    {
        m_backButton.onClick.AddListener(OnBack);
        m_hostButton.onClick.AddListener(OnHost);
        m_joinButton.onClick.AddListener(OnJoin);

        m_clientJoinBtn.onClick.AddListener(OnClientJoin);
        m_clientBackBtn.onClick.AddListener(OnClientBack);

        m_hostCancelBtn.onClick.AddListener(OnHostBack);

        CustomNetworkManager.OnPlayersConnected = OnSuccess;
    }

    private void WaitForNetwork(bool _wait)
    {
        m_spinner.gameObject.SetActive(_wait);
        m_waitingForNetwork = _wait;
    }

    private void OnHost()
    {
        manager.StartHost();
        m_startScreen.SetActive(false);
        m_hostScreen.SetActive(true);
        WaitForNetwork(true);
    }

    private void OnJoin()
    {
        m_startScreen.SetActive(false);
        m_clientScreen.SetActive(true);
    }

    private void OnBack()
    {
        m_startScreen.SetActive(false);
    }

    private void OnClientJoin()
    {
        manager.networkAddress = m_inputField.text;
        manager.StartClient();
        WaitForNetwork(true);
    }

    private void OnClientBack()
    {
        m_startScreen.SetActive(true);
        m_clientScreen.SetActive(false);
        WaitForNetwork(false);
    }

    private void OnHostBack()
    {
        m_startScreen.SetActive(true);
        m_hostScreen.SetActive(false);

        manager.StopHost();
        WaitForNetwork(false);
    }

    private void OnError()
    {
        WaitForNetwork(false);
    }

    private void OnSuccess()
    {
        if (!m_waitingForNetwork) return;

        WaitForNetwork(false);

        m_mainMenuController.Play();

        gameObject.SetActive(false);
    }
}
