using UnityEngine;

public class Initialization : MonoBehaviour
{
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
        AuthHandler.Instance.Authenticate(FinishInit);
    }
    
    private void FinishInit()
    {
        SceneManager.LoadMainMenu();
    }
}
