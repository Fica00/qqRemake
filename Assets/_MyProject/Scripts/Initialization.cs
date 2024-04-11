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

    public void CheckForStartingData()
    {
        if (DataManager.Instance.PlayerData == null || DataManager.Instance.PlayerData.Decks == null)
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
        SceneManager.Instance.LoadMainMenu();
        return;
        
        if (DataManager.Instance.PlayerData.HasFinishedTutorial==0 && DataManager.Instance.PlayerData.Exp==0)
        {
            DataManager.Instance.Subscribe();
            SceneManager.Instance.LoadTutorial();
        }
        else
        {
            SceneManager.Instance.LoadMainMenu();
        }
    }
}
