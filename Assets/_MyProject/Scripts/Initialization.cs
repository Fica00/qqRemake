using UnityEngine;
using UnityEngine.UI;

public class Initialization : MonoBehaviour
{
    public static Initialization Instance;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject startHolder;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(EnterFullScreen);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(EnterFullScreen);
    }


    private void EnterFullScreen()
    {
        JavaScriptManager.Instance.EnterFullScreen();
        InitPhoton();
        startHolder.SetActive(false);
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
                    UIManager.Instance.OkDialog.Setup("Something went wrong while setting starting data");
                }
            });
            return;
        }
        FinishInit();
    }
    
    private void FinishInit()
    {
        SceneManager.LoadMainMenu();
    }
}
