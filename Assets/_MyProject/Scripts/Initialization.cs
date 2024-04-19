using UnityEngine;

public class Initialization : MonoBehaviour
{
    public static Initialization Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RankSo.Init();
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

    public void CheckForStartingData(bool _isNewAccount)
    {
        if (_isNewAccount)
        {
            FirebaseManager.Instance.SetStartingData((_status) =>
            {
                if (_status)
                {
                    FinishInit();
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

        if (DataManager.Instance.PlayerData.HasFinishedTutorial==0 && DataManager.Instance.PlayerData.Exp==0)
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
