using System.Linq;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    public static Initialization Instance;
    
    [SerializeField] private PwaOverlayHandler pwaOverlayHandler;

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
        TryShowPwaOverlay();
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
                    FinishInit();

                    DataManager.Instance.PlayerData.Agency = _agency;
                    DataManager.Instance.PlayerData.IsGuest = _isGuest;
                    DataManager.Instance.PlayerData.IsNewAccount = true;

                    DataManager.Instance.SaveIsGuest();

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
            BotPlayer.GenerateNewData();
            SceneManager.Instance.LoadAIGameplay(false);
            return;
        }
        
        DataManager.Instance.CanShowPwaOverlay = true;
        SceneManager.Instance.LoadMainMenu();
    }
}