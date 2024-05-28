using Newtonsoft.Json;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    public static Initialization Instance;

    private void Awake()
    {
        Instance = this;
        Debug.developerConsoleVisible = false;
    }

    private void Start()
    {
        RankSo.Init();
        if (JavaScriptManager.Instance.IsDemo)
        {
            InitDataManager();
            return;
        }

        InitPhoton();
    }


    private void InitPhoton()
    {
        PhotonManager.OnFinishedInit += InitDataManager;
        PhotonManager.Instance.Init();
    }

    private void InitDataManager()
    {
        PhotonManager.OnFinishedInit -= InitDataManager;
        AuthHandler.Instance.Authenticate();
    }

    public void CheckForStartingData(bool _isNewAccount, string _agency)
    {
        if (_isNewAccount)
        {
            FirebaseManager.Instance.SetStartingData((_status) =>
            {
                if (_status)
                {
                    AuthOnServer();
                    DataManager.Instance.PlayerData.Agency = _agency;
                }
                else
                {
                    DialogsManager.Instance.OkDialog.Setup("Something went wrong while setting starting data");
                }
            });
            return;
        }

        AuthOnServer();
    }

    private void AuthOnServer()
    {
        HttpCommunicationHandler.Instance.Authenticate(FirebaseManager.Instance.PlayerId, HandleServerAuthFinished);
    }

    private void HandleServerAuthFinished(bool _status, string _data)
    {
        if (!_status)
        {
            DialogsManager.Instance.OkDialog.Setup("Something went wrong while connecting to the server, try again later");
            return;
        }

        AuthToken _token = JsonConvert.DeserializeObject<AuthToken>(_data);
        if (_token.Token == "Invalid user")
        {
            DialogsManager.Instance.OkDialog.Setup("Invalid user during auth, try again later");
            return;
        }
        SocketServerCommunication.Instance.SetAuthToken(_token.Token);
        FinishInit();
    }

    private void FinishInit()
    {
        AudioManager.Instance.Init();

        if (DataManager.Instance.PlayerData.HasFinishedTutorial == 0 && DataManager.Instance.PlayerData.Exp == 0)
        {
            DataManager.Instance.Subscribe();
            SceneManager.Instance.LoadSimpleTutorial();
        }
        else
        {
            SceneManager.Instance.LoadMainMenu();
        }
    }
}