using UnityEngine;
using UnityEngine.UI;

public class GuestOverlayHandler : OverlayHandler
{
    [SerializeField] private Button settingsButton;
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        settingsButton.onClick.AddListener(ShowSettings);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        settingsButton.onClick.RemoveListener(ShowSettings);
    }
    
    private void ShowSettings()
    {
        Close();
        SceneManager.Instance.LoadSettingsPage();
    }

    public override void Close()
    {
        base.Close();
        DataManager.Instance.PlayerData.GuestOverlayShown = true;
    }
}