using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class AuthHandler : MonoBehaviour
{
    public static AuthHandler Instance;
    [SerializeField] private RegisterHandler registerHandler;
    private Action<bool> callBackForOAUTh;
    
    public static bool CanAuth;
    public static bool IsGuest = true;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Authenticate()
    {
        CanAuth = true;
        if (Application.isEditor)
        {
            StartCoroutine(ShowRegisterRoutine());
            return;
        }
        
        JavaScriptManager.OnGotUserData += TryToLogin;
        JavaScriptManager.Instance.RequestUserData();
    }

    private void TryToLogin(string _loginDataJson)
    {
        JavaScriptManager.OnGotUserData -= TryToLogin;
        UserLoginData _loginData = default;
        
        if (!string.IsNullOrEmpty(_loginDataJson))
        {
            _loginData = JsonConvert.DeserializeObject<UserLoginData>(_loginDataJson);
        }

        if (_loginData == null || string.IsNullOrEmpty(_loginData.UserId))
        {
            StartCoroutine(ShowRegisterRoutine());
        }
        else
        {
            Auth(_loginData.UserId, _loginData.IsNewAccount, _loginData.Agency);
        }
    }


    private IEnumerator ShowRegisterRoutine()
    {
        yield return new WaitForSeconds(3);
        if (!gameObject.activeSelf)
        {
            yield break;
        }

        if (registerHandler == default)
        {
            yield break;
        }

        registerHandler.Setup();
    }

    public void LoginWithTwitter(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        JavaScriptManager.Instance.LoginTwitter();
        IsGuest = false;
    }    
    
    public void LoginWithDiscord(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        JavaScriptManager.Instance.LoginWithDiscord();
        IsGuest = false;
    }

    public void LoginWithGoogle(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        JavaScriptManager.Instance.GoogleAuth();
        IsGuest = false;
    }
    
    public void AnonymousSignIn(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        if (Application.isEditor)
        {
            FirebaseManager.Instance.TryLoginAndGetData("unity22@help.com", "unity123", (_status) =>
            {
                HandleLoginResult(_status,callBackForOAUTh,false);
            });
            IsGuest = true;
        }
        else
        {
            JavaScriptManager.Instance.AnonymousAuth();
            IsGuest = true;
        }
    }

    public void Auth(string _id, bool _isNewAccount, string _agency="")
    {
        FirebaseManager.Instance.SignIn(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh, _isNewAccount, _agency);
        });
    }

    private void HandleLoginResult(bool _status, Action<bool> _callBack, bool _isNewAccount, string _agency = "")
    {
        if (!_status)
        {
            _callBack?.Invoke(false);
            return;
        }

        if (!CanAuth)
        {
            return;
        }

        CanAuth = false;
        JavaScriptManager.Instance.SetUserId(FirebaseManager.Instance.PlayerId);
        if (Initialization.Instance==default)
        {
            return;
        }
        
        Initialization.Instance.CheckForStartingData(_isNewAccount, _agency);
    }

    public void AuthFailed()
    {
        callBackForOAUTh?.Invoke(false);
    }
}
