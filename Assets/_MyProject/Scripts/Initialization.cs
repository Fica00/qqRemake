using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    public static Initialization Instance;
    
    [SerializeField] private PwaOverlayHandler pwaOverlayHandler;

    private void Awake()
    {
        Application.runInBackground = true;
        Instance = this;
        Debug.developerConsoleVisible = false;
    }

    private void Start()
    {
        RankSo.Init();
        InitDataManager();
        TryShowPwaOverlay();
    }

    private void InitDataManager()
    {
        AuthHandler.Instance.Authenticate();
    }

    private void TryShowPwaOverlay()
    {
        if(JavaScriptManager.Instance.IsPwaPlatform)
        {
            return;
        }

        if (JavaScriptManager.Instance.IsOnPc())
        {
            return;
        }
      
        pwaOverlayHandler.SetupWithText(JavaScriptManager.Instance.IsAndroid());
    }

    public void CheckForStartingData(bool _isNewAccount, bool _isGuest, string _agency)
    {
        if (_isNewAccount)
        {
            FirebaseManager.Instance.SetStartingData((_status) =>
            {
                if (_status)
                {
                    AuthOnServer();
                    DataManager.Instance.PlayerData.Agency = _agency;
                    DataManager.Instance.PlayerData.IsGuest = _isGuest;
                    DataManager.Instance.PlayerData.IsNewAccount = true;

                    if (AgencyManager.Instance.DoesAgencyExist(_agency))
                    {
                        DataManager.Instance.PlayerData.Exp =
                            PlayerData.ExpBorders[AgencyManager.Instance.Agencies.First(_ => _.Name == _agency).Level - 1];
                    }
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
        DataManager.Instance.SaveIsGuest();
    }

    private void FinishInit()
    {
        AudioManager.Instance.Init();
        DataManager.Instance.Subscribe();
        MissionManager.Instance.Setup();
        DataManager.Instance.PlayerData.Statistics.StartSession();
        
        if (!DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            SceneManager.Instance.LoadTutorial(false);
            return;
        }
        
        DataManager.Instance.CanShowPwaOverlay = true;
        SceneManager.Instance.LoadMainMenu();
    }
}