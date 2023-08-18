using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RegisterHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private Button loginWithEmail;
    [SerializeField] private Button loginWithGoogle;
    [SerializeField] private Button loginWithFacebook;
    
    
    public void Setup()
    {
        emailInput.text = string.Empty;
        passwordInput.text = string.Empty;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        loginWithEmail.onClick.AddListener(LoginWithEmail);
        loginWithFacebook.onClick.AddListener(LoginWithFacebook);
        loginWithGoogle.onClick.AddListener(LoginWithGoogle);
    }

    private void OnDisable()
    {
        loginWithEmail.onClick.RemoveListener(LoginWithEmail);
        loginWithFacebook.onClick.RemoveListener(LoginWithFacebook);
        loginWithGoogle.onClick.RemoveListener(LoginWithGoogle);
    }

    private void LoginWithEmail()
    {
        string _email = emailInput.text;
        string _password = passwordInput.text;
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
        emailInput.interactable = _status;
        passwordInput.interactable = _status;
        loginWithEmail.interactable = _status;
        loginWithGoogle.interactable = _status;
        loginWithFacebook.interactable = _status;
    }
}
