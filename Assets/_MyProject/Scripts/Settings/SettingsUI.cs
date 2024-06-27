using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button redeemCode;
    [SerializeField] private Button reportABug;
    [SerializeField] private Button playerSupport;
    [SerializeField] private Button privacy;
    [SerializeField] private Button termsOfService;
    [SerializeField] private Button deleteAccount;
    [SerializeField] private Button close;
    [SerializeField] private Button tutorial;
    [SerializeField] private TextMeshProUGUI playerIDDisplay;
    [SerializeField] private Button copyPlayerId;
    [SerializeField] private Button profile;
    [SerializeField] private SocialOverlayHandler socialOverlayHandler;


    private void OnEnable()
    {
        logoutButton.onClick.AddListener(Logout);
        redeemCode.onClick.AddListener(RedeemCode);
        reportABug.onClick.AddListener(ReportABug);
        playerSupport.onClick.AddListener(PlayerSupport);
        privacy.onClick.AddListener(Privacy);
        termsOfService.onClick.AddListener(TermsOfService);
        deleteAccount.onClick.AddListener(DeleteAccount);
        close.onClick.AddListener(Close);
        tutorial.onClick.AddListener(ShowTutorial);
        copyPlayerId.onClick.AddListener(CopyPlayerId);
        profile.onClick.AddListener(ShowProfile);

        playerIDDisplay.text = "Player id: " + FirebaseManager.Instance.PlayerId;
    }

    private void OnDisable()
    {
        logoutButton.onClick.RemoveListener(Logout);
        redeemCode.onClick.RemoveListener(RedeemCode);
        reportABug.onClick.RemoveListener(ReportABug);
        playerSupport.onClick.RemoveListener(PlayerSupport);
        privacy.onClick.RemoveListener(Privacy);
        termsOfService.onClick.RemoveListener(TermsOfService);
        deleteAccount.onClick.RemoveListener(DeleteAccount);
        close.onClick.RemoveListener(Close);
        tutorial.onClick.RemoveListener(ShowTutorial);
        copyPlayerId.onClick.RemoveListener(CopyPlayerId);
        profile.onClick.RemoveListener(ShowProfile);
    }

    private void CopyPlayerId()
    {
        JavaScriptManager.Instance.CopyToClipboard(FirebaseManager.Instance.PlayerId);
    }

    private void ShowTutorial()
    {
        SceneManager.Instance.LoadTutorial();
    }

    private void Logout()
    {
        StartCoroutine(LogOutRoutine());

        IEnumerator LogOutRoutine()
        {
            PlayerPrefs.DeleteAll();
            FirebaseManager.Instance.SignOut();
            if (Application.isEditor)
            {
                SceneManager.Instance.LoadAlphaCode();
                yield break;
            }

            JavaScriptManager.Instance.SignOut();
            yield return new WaitForSeconds(1);
            SceneManager.Instance.LoadAlphaCode();
        }
    }

    private void RedeemCode()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void ReportABug()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void PlayerSupport()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void Privacy()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void TermsOfService()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void DeleteAccount()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void Start()
    {

        JavaScriptManager.Instance.CheckHasBoundAccount(_hasBoundAccount =>
        {
            RegisterAnonymousHandler.Instance.CheckIsGuest();
            if (_hasBoundAccount)
            {
                return;
            }

            socialOverlayHandler.Setup();
        });
        
    }

    private void Close()
    {
        if (DataManager.Instance.PlayerData.SettingsFirstTimeShown == PwaOverlay.DidNotOpen)
        {
            DataManager.Instance.PlayerData.SettingsFirstTimeShown = PwaOverlay.DidNotShow;
        }

        SceneManager.Instance.LoadMainMenu();
    }
    
    private void ShowProfile()
    {
        SceneManager.Instance.LoadProfile();
    }
}