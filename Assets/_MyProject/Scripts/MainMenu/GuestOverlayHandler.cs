using UnityEngine;
using UnityEngine.UI;

public class GuestOverlayHandler : OverlayHandler
{
    private const string CHECK_POINT = "entered game";
    [SerializeField] private Button settingsButton;
    
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Hide);
        settingsButton.onClick.AddListener(ShowSettings);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Hide);
        settingsButton.onClick.RemoveListener(ShowSettings);
    }
    
    private void ShowSettings()
    {
        DataManager.Instance.PlayerData.Statistics.NoteCheckPoint(CHECK_POINT, "Selected settings");
        Close();
        SceneManager.Instance.LoadSettingsPage();
    }

    private void Hide()
    {
        DataManager.Instance.PlayerData.Statistics.NoteCheckPoint(CHECK_POINT, "Selected play");
        Close();
    }

    public override void Close()
    {
        base.Close();
        DataManager.Instance.PlayerData.GuestOverlayShown = true;
    }
}