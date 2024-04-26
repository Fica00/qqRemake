using UnityEngine;
using UnityEngine.UI;

public class RegisterHandler : MonoBehaviour
{
    [SerializeField] private Button loginWithGoogle;
    [SerializeField] private Button loginTwitter;
    [SerializeField] private Button loginDiscord;
    [SerializeField] private Button guestButton;

    [SerializeField] private GameObject loginAnimation;
    
    public void Setup()
    {
        loginAnimation.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        loginTwitter.onClick.AddListener(LoginTwitter);
        loginWithGoogle.onClick.AddListener(LoginWithGoogle);
        guestButton.onClick.AddListener(LoginAsGuest);
        loginDiscord.onClick.AddListener(LoginDiscord);
    }

    private void OnDisable()
    {
        loginTwitter.onClick.RemoveListener(LoginTwitter);
        loginWithGoogle.onClick.RemoveListener(LoginWithGoogle);
        guestButton.onClick.RemoveListener(LoginAsGuest);
        loginDiscord.onClick.AddListener(LoginDiscord);
    }
    
    private void LoginAsGuest()
    {
        ManageIntractables(false);
        AuthHandler.Instance.AnonymousSignIn(HandleLoginResult);
    }

    private void LoginWithGoogle()
    {
        ManageIntractables(false);
        AuthHandler.Instance.LoginWithGoogle(HandleLoginResult);
    }

    private void LoginTwitter()
    {
        ManageIntractables(false);
        AuthHandler.Instance.LoginWithTwitter(HandleLoginResult);
    }
    
    private void LoginDiscord()
    {
        AuthHandler.Instance.LoginWithDiscord(HandleLoginResult);
    }


    private void HandleLoginResult(bool _result)
    {
        if (_result)
        {
            return;
        }

        ManageIntractables(true);
    }

    private void ManageIntractables(bool _status)
    {
        loginWithGoogle.interactable = _status;
        loginTwitter.interactable = _status;
        guestButton.interactable = _status;
        loginDiscord.interactable = _status;
    }
}
