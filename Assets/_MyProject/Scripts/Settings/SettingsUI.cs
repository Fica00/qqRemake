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

    private void OnEnable()
    {
        logoutButton.onClick.AddListener(Logout);
        linkWithFacebook.onClick.AddListener(LinkWithFacebook);
        linkWithGoogle.onClick.AddListener(LinkWithGoogle);
        redeemCode.onClick.AddListener(RedeemCode);
        reportABug.onClick.AddListener(ReportABug);
        playerSupport.onClick.AddListener(PlayerSupport);
        privacy.onClick.AddListener(Privacy);
        termsOfService.onClick.AddListener(TermsOfService);
        deleteAccount.onClick.AddListener(DeleteAccount);
        close.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        logoutButton.onClick.RemoveListener(Logout);
        linkWithFacebook.onClick.RemoveListener(LinkWithFacebook);
        linkWithGoogle.onClick.RemoveListener(LinkWithGoogle);
        redeemCode.onClick.AddListener(RedeemCode);
        reportABug.onClick.AddListener(ReportABug);
        playerSupport.onClick.AddListener(PlayerSupport);
        privacy.onClick.AddListener(Privacy);
        termsOfService.onClick.AddListener(TermsOfService);
        deleteAccount.onClick.AddListener(DeleteAccount);
        close.onClick.AddListener(Close);
    }

    private void Logout()
    {
        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       JavaScriptManager.Instance.LoadURL(JavaScriptManager.Instance.GameLink);
#endif
    }

    private void LinkWithFacebook()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void LinkWithGoogle()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }
    
    private void RedeemCode()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void ReportABug()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void PlayerSupport()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void Privacy()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void TermsOfService()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void DeleteAccount()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    private void Start()
    {
        nameInput.text = DataManager.Instance.PlayerData.Name;
    }

    private void Close()
    {
        if (!TryUpdateName())
        {
            return;
        }
        
        SceneManager.Instance.LoadMainMenu();
    }

    private bool TryUpdateName()
    {
        string _name = nameInput.text;
        if (string.IsNullOrEmpty(_name))
        {
            UIManager.Instance.OkDialog.Setup("Please enter name");
            return false;
        }

        if (_name.Length<3 || _name.Length>10)
        {
            UIManager.Instance.OkDialog.Setup("Name must contain more than 3 characters and less than 10");
            return false;
        }
        
        if (DataManager.Instance.PlayerData.Name==_name)
        {
            return true;
        }

        DataManager.Instance.PlayerData.Name = _name;
        return true;
    }
}
