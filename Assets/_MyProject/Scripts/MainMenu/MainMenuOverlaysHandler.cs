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
        // if (DataManager.Instance.PlayerData.IsNewAccount)
        // {
        //     SetupFirstTimeOverlays();
        // }

        SetupOverlays();
    }

    private void SetupOverlays()
    {
        if (JavaScriptManager.Instance.IsPwaPlatform)
        {
            return;
        }
        
        if (JavaScriptManager.Instance.IsOnPc())
        {
            return;
        }

        if (!DataManager.Instance.CanShowPwaOverlay)
        {
            return;
        }

        pwaOverlayHandler.SetupWithText(JavaScriptManager.Instance.IsAndroid());
    }

    private void SetupFirstTimeOverlays()
    {
        if (DataManager.Instance.PlayerData.HasPlayedFirstGame && !DataManager.Instance.PlayerData.GuestOverlayShown)
        {
            DataManager.Instance.PlayerData.GuestOverlayShown = true;
            Debug.Log("Setting up Guest overlay.");
            guestOverlay.Setup();
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
                pwaOverlayHandler.SetupWithText(JavaScriptManager.Instance.IsAndroid());
                break;
        }
    }
}