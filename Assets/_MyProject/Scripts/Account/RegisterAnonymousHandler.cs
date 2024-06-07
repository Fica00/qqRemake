using UnityEngine;
using UnityEngine.UI;

public class RegisterAnonymousHandler : MonoBehaviour
{
    public static RegisterAnonymousHandler Instance;

    public GameObject RegistrationPageHolder;

    [SerializeField] private Button googleButton;
    [SerializeField] private Button twitterButton;

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
        
        RegistrationPageHolder.SetActive(false);
        CheckIsGuest();
    }


    private void OnDisable()
    {
        googleButton.onClick.RemoveListener(() => LinkUserProvider("google"));
        twitterButton.onClick.RemoveListener(() => LinkUserProvider("twitter"));
    }

    private void LinkUserProvider(string _providerName)
    {
        JavaScriptManager.Instance.LinkingAnonimousUser(_providerName);
    }

    private void CheckIsGuest()
    {
        JavaScriptManager.Instance.CheckHasBoundAccount(DoManageButtons);
        
        void DoManageButtons(bool _didBind)
        {
            RegistrationPageHolder.SetActive(!_didBind);
        }
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