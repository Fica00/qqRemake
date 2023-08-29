using System;
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
        if (DataManager.Instance.PlayerData == null || string.IsNullOrEmpty(DataManager.Instance.PlayerData.Name))
        {
            Debug.Log("Detected empty name");
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
        Debug.Log("123");
        FinishInit();
    }
    
    private void FinishInit()
    {
        SceneManager.LoadMainMenu();
    }
}
