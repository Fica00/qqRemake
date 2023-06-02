using UnityEngine;

public class Initialization : MonoBehaviour
{
    private void Start()
    {
        //InitPhoton();
        InitDataManager();
    }

    void InitPhoton()
    {
        PhotonManager.OnFinishedInit += InitDataManager;
        PhotonManager.Instance.Init();
    }

    void InitDataManager()
    {
        //PhotonManager.OnFinishedInit -= InitDataManager;
        DataManager.Instance.Init(FinishInit);
    }

    void FinishInit()
    {
        SceneManager.LoadMainMenu();
    }
}
