using UnityEngine;
using UnityEngine.UI;

public class RegisterAnonymousHandler : MonoBehaviour
{
    public static RegisterAnonymousHandler Instance;

    public GameObject RegistrationPageHolder;

    [SerializeField] private Button googleButton;
    [SerializeField] private Button twitterButton;
    [SerializeField] private Button googleButtonOverlay;
    [SerializeField] private Button twitterButtonOverlay;

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

    private void OnEnable()
    {
        googleButton.onClick.AddListener(() => LinkUserProvider("google"));
        twitterButton.onClick.AddListener(() => LinkUserProvider("twitter"));
        googleButtonOverlay.onClick.AddListener(() => LinkUserProvider("google"));
        twitterButtonOverlay.onClick.AddListener(() => LinkUserProvider("twitter"));
        
        RegistrationPageHolder.SetActive(false);
    }


    private void OnDisable()
    {
        googleButton.onClick.RemoveListener(() => LinkUserProvider("google"));
        twitterButton.onClick.RemoveListener(() => LinkUserProvider("twitter"));
        googleButtonOverlay.onClick.RemoveListener(() => LinkUserProvider("google"));
        twitterButtonOverlay.onClick.RemoveListener(() => LinkUserProvider("twitter"));
    }

    private void LinkUserProvider(string _providerName)
    {
        JavaScriptManager.Instance.LinkingAnonimousUser(_providerName);
    }

    public void CheckIsGuest()
    {
        JavaScriptManager.Instance.CheckHasBoundAccount(DoManageButtons);
    }
    
    private void DoManageButtons(bool _didBind)
    {
        RegistrationPageHolder.SetActive(!_didBind);
    }

    public void HideRegistrationPage()
    {
        RegistrationPageHolder.gameObject.SetActive(false);
    }

    public void UserAlreadyHaveAccount()
    {
        DialogsManager.Instance.OkDialog.Setup("You already have a linked account.");
        HideRegistrationPage();
    }
}