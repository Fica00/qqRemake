using UnityEngine;

public enum MainMenuOverlay
{
    Guest,
    FirstTimePlay,
    ClaimQoomons
}

public class MainMenuOverlaysHandler : MonoBehaviour
{
    public static MainMenuOverlaysHandler Instance;

    [SerializeField] private GuestOverlayHandler guestOverlay;
    [SerializeField] private FirstTimePlayOverlay firstTimePlayOverlay;
    [SerializeField] private ClaimQoomonsOverlay claimQoomonsOverlay;

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
        Debug.Log("BeforeFirstGameOverlayShown: " + DataManager.Instance.PlayerData.BeforeFirstGameOverlayShown);
        Debug.Log("AfterFirstGameOverlayShown: " + DataManager.Instance.PlayerData.AfterFirstGameOverlayShown);
        Debug.Log("GuestOverlayShown: " + DataManager.Instance.PlayerData.GuestOverlayShown);

        if (!DataManager.Instance.PlayerData.BeforeFirstGameOverlayShown && !DataManager.Instance.PlayerData.AfterFirstGameOverlayShown)
        {
            DataManager.Instance.PlayerData.BeforeFirstGameOverlayShown = true;
            Debug.Log("Setting up FirstTimePlay overlay.");
            firstTimePlayOverlay.Setup();
            return;
        }

        if (DataManager.Instance.PlayerData.BeforeFirstGameOverlayShown && !DataManager.Instance.PlayerData.AfterFirstGameOverlayShown)
        {
            DataManager.Instance.PlayerData.AfterFirstGameOverlayShown = true;
            Debug.Log("Setting up ClaimQoomons overlay.");
            claimQoomonsOverlay.Setup();
            return;
        }

        if (DataManager.Instance.PlayerData.BeforeFirstGameOverlayShown && DataManager.Instance.PlayerData.AfterFirstGameOverlayShown && !DataManager.Instance.PlayerData.GuestOverlayShown)
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
        }
    }
}