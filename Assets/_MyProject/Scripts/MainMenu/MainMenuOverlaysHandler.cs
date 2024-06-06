using System;
using UnityEngine;

public enum MainMenuOverlay
{
    Guest,
    FirstTimePlay,
    ClaimQoomons,
    PwaOverlay
}

public class MainMenuOverlaysHandler : MonoBehaviour
{
    public static MainMenuOverlaysHandler Instance;

    [SerializeField] private GuestOverlayHandler guestOverlay;
    [SerializeField] private FirstTimePlayOverlay firstTimePlayOverlay;
    [SerializeField] private ClaimQoomonsOverlay claimQoomonsOverlay;
    [SerializeField] private PwaOverlayHandler pwaOverlayHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupOverlays();
    }

    private void SetupOverlays()
    {
        if (DataManager.Instance.PlayerData.HasPlayedFirstGame && !DataManager.Instance.PlayerData.AfterFirstGameOverlayShown)
        {
            DataManager.Instance.PlayerData.AfterFirstGameOverlayShown = true;
            Debug.Log("Setting up ClaimQoomons overlay.");
            claimQoomonsOverlay.Setup();
            return;
        }

        if (DataManager.Instance.PlayerData.HasPlayedFirstGame && DataManager.Instance.PlayerData.AfterFirstGameOverlayShown && !DataManager.Instance.PlayerData.GuestOverlayShown)
        {
            DataManager.Instance.PlayerData.GuestOverlayShown = true;
            Debug.Log("Setting up Guest overlay.");
            guestOverlay.Setup();
        }

        if (DataManager.Instance.PlayerData.SettingsFirstTimeShown == PwaOverlay.DidNotShow)
        {
            DataManager.Instance.PlayerData.SettingsFirstTimeShown = PwaOverlay.Showed;
            pwaOverlayHandler.SetupWithText(JavaScriptManager.Instance.IsAndroid);
        }
            
    }

    public void SetupOverlay(MainMenuOverlay _overlay)
    {
        switch (_overlay)
        {
            case MainMenuOverlay.Guest:
                guestOverlay.Setup();
                break;
            case MainMenuOverlay.FirstTimePlay:
                firstTimePlayOverlay.Setup();
                break;
            case MainMenuOverlay.ClaimQoomons:
                claimQoomonsOverlay.Setup();
                break;
            case MainMenuOverlay.PwaOverlay:
                pwaOverlayHandler.SetupWithText(JavaScriptManager.Instance.IsAndroid);
                break;
        }
    }
}