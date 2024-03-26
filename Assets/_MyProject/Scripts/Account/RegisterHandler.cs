using UnityEngine;
using UnityEngine.UI;

public class RegisterHandler : MonoBehaviour
{
    [SerializeField] private Button loginWithGoogle;
    [SerializeField] private Button loginWithFacebook;
    [SerializeField] private Button guestButton;

    [SerializeField] private GameObject loginAnimation;
    
    public void Setup()
    {
        loginAnimation.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        loginWithFacebook.onClick.AddListener(LoginWithFacebook);
        loginWithGoogle.onClick.AddListener(LoginWithGoogle);
        guestButton.onClick.AddListener(LoginAsGuest);
    }

    private void OnDisable()
    {
        loginWithFacebook.onClick.RemoveListener(LoginWithFacebook);
        loginWithGoogle.onClick.RemoveListener(LoginWithGoogle);
        guestButton.onClick.RemoveListener(LoginAsGuest);
    }

    private void LoginAsGuest()
    {
        string _email = "guest"+System.Guid.NewGuid()+"@help.com";
        string _password = "paSsword123";
        if (CredentialsValidator.VerifyEmail(_email) && CredentialsValidator.VerifyPassword(_password))
        {
            ManageIntractables(false);
            AuthHandler.Instance.LoginWithEmail(_email,_password,HandleLoginResult);
        }
    }

    private void LoginWithGoogle()
    {
        ManageIntractables(false);
        AuthHandler.Instance.LoginWithGoogle(HandleLoginResult);
    }

    private void LoginWithFacebook()
    {
        ManageIntractables(false);
        AuthHandler.Instance.LoginWithFacebook(HandleLoginResult);
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
        loginWithFacebook.interactable = _status;
        guestButton.interactable = _status;
    }
}
