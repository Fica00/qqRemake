using UnityEngine;
using UnityEngine.UI;

public class ClaimQoomonsOverlay : OverlayHandler
{
    [SerializeField] private Button missionButton;
    [SerializeField] private Button levelButton;
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        missionButton.onClick.AddListener(ShowMissionsPage);
        levelButton.onClick.AddListener(ShowLevelPage);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        missionButton.onClick.RemoveListener(ShowMissionsPage);
        levelButton.onClick.RemoveListener(ShowLevelPage);
    }
    
    public override void Close()
    {
        base.Close();
        MainMenuOverlaysHandler.Instance.SetupOverlay(MainMenuOverlay.Guest);
    }

    private void ShowLevelPage()
    {
        Close();
        SceneManager.Instance.LoadLevelPage();
    }
    
    private void ShowMissionsPage()
    {
        Close();
        SceneManager.Instance.LoadMissionsPage();
    }

}