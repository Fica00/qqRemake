using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button logoutButton;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button linkWithFacebook;
    [SerializeField] private Button linkWithGoogle;
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
    [SerializeField] private SocialOverlayHandler socialOverlayHandler;


    private void OnEnable()
    {
        logoutButton.onClick.AddListener(Logout);
        //linkWithFacebook.onClick.AddListener(LinkWithFacebook); => Rekao dule da se izbacuje Facebook
        //linkWithGoogle.onClick.AddListener(LinkWithGoogle);
        redeemCode.onClick.AddListener(RedeemCode);
        reportABug.onClick.AddListener(ReportABug);
        playerSupport.onClick.AddListener(PlayerSupport);
        privacy.onClick.AddListener(Privacy);
        termsOfService.onClick.AddListener(TermsOfService);
        deleteAccount.onClick.AddListener(DeleteAccount);
        close.onClick.AddListener(Close);
        tutorial.onClick.AddListener(ShowTutorial);
        copyPlayerId.onClick.AddListener(CopyPlayerId);

        playerIDDisplay.text = "Player id: " + FirebaseManager.Instance.PlayerId;
    }

    private void OnDisable()
    {
        logoutButton.onClick.RemoveListener(Logout);
        //linkWithFacebook.onClick.RemoveListener(LinkWithFacebook);
        //linkWithGoogle.onClick.RemoveListener(LinkWithGoogle);
        redeemCode.onClick.RemoveListener(RedeemCode);
        reportABug.onClick.RemoveListener(ReportABug);
        playerSupport.onClick.RemoveListener(PlayerSupport);
        privacy.onClick.RemoveListener(Privacy);
        termsOfService.onClick.RemoveListener(TermsOfService);
        deleteAccount.onClick.RemoveListener(DeleteAccount);
        close.onClick.RemoveListener(Close);
        tutorial.onClick.RemoveListener(ShowTutorial);
        copyPlayerId.onClick.RemoveListener(CopyPlayerId);
    }

    private void CopyPlayerId()
    {
        JavaScriptManager.Instance.CopyToClipboard(FirebaseManager.Instance.PlayerId);
    }

    private void ShowTutorial()
    {
        SceneManager.Instance.LoadSimpleTutorial();
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

    private void LinkWithFacebook()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void LinkWithGoogle()
    {
        DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
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
        nameInput.text = DataManager.Instance.PlayerData.Name;

        JavaScriptManager.Instance.CheckHasBoundAccount(_hasBoundAccount =>
        {
            if (_hasBoundAccount)
            {
                return;
            }

            socialOverlayHandler.Setup();
        });
    }

    private void Close()
    {
        if (!TryUpdateName())
        {
            return;
        }

        if (DataManager.Instance.PlayerData.SettingsFirstTimeShown == PwaOverlay.DidNotOpen)
        {
            DataManager.Instance.PlayerData.SettingsFirstTimeShown = PwaOverlay.DidNotShow;
        }

        SceneManager.Instance.LoadMainMenu();
    }

    private bool TryUpdateName()
    {
        string _name = nameInput.text;
        if (string.IsNullOrEmpty(_name))
        {
            DialogsManager.Instance.OkDialog.Setup("Please enter name");
            return false;
        }

        if (_name.Length < 3 || _name.Length > 10)
        {
            DialogsManager.Instance.OkDialog.Setup("Name must contain more than 3 characters and less than 10");
            return false;
        }

        if (DataManager.Instance.PlayerData.Name == _name)
        {
            return true;
        }

        DataManager.Instance.PlayerData.Name = _name;
        return true;
    }
}