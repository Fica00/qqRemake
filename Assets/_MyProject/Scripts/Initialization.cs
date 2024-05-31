using System.Linq;
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
                    FinishInit();
                    DataManager.Instance.PlayerData.Agency = _agency;
                    
                    if (AgencyManager.Instance.DoesAgencyExist(_agency))
                    {
                        DataManager.Instance.PlayerData.Exp = PlayerData.ExpBorders[AgencyManager.Instance.Agencies.First(_ => _.Name == _agency).Level - 1];
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