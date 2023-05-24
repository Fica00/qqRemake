using UnityEngine;

public class Initialization : MonoBehaviour
{
    private void Start()
    {
        InitPhoton();
    }

    void InitPhoton()
    {
        PhotonManager.OnFinishedInit += FinishInit;
        PhotonManager.Instance.Init();
    }

    void FinishInit()
    {
        PhotonManager.OnFinishedInit -= FinishInit;

        SceneManager.LoadMainMenu();
    }
}
