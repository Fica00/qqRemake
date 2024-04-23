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
            Auth(_loginData.UserId, _loginData.IsNewAccount);
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

    public void LoginWithFacebook(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        JavaScriptManager.Instance.FacebookAuth();
    }

    public void LoginWithGoogle(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        JavaScriptManager.Instance.GoogleAuth();
    }
    
    public void AnonymousSignIn(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        if (Application.isEditor)
        {
            FirebaseManager.Instance.TryLoginAndGetData("unity@help.com", "unity123", (_status) =>
            {
                HandleLoginResult(_status,callBackForOAUTh,false);
            });
        }
        else
        {
            JavaScriptManager.Instance.AnonymousAuth();
        }
    }

    public void Auth(string _id, bool _isNewAccount)
    {
        FirebaseManager.Instance.SignIn(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh, _isNewAccount);
        });
    }

    private void HandleLoginResult(bool _status, Action<bool> _callBack, bool _isNewAccount)
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
        
        Initialization.Instance.CheckForStartingData(_isNewAccount);
    }

    public void AuthFailed()
    {
        callBackForOAUTh?.Invoke(false);
    }
}
