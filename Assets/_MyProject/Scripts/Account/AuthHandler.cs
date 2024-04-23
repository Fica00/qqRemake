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
        
        Debug.Log("111111");
        JavaScriptManager.OnGotUserData += TryToLogin;
        Debug.Log("222222");
        JavaScriptManager.Instance.RequestUserData();
    }

    private void TryToLogin(string _loginDataJson)
    {
        Debug.Log("kkk");
        JavaScriptManager.OnGotUserData -= TryToLogin;
        UserLoginData _loginData = default;
        
        if (!string.IsNullOrEmpty(_loginDataJson))
        {
            Debug.Log("aaa");
            _loginData = JsonConvert.DeserializeObject<UserLoginData>(_loginDataJson);
        }

        if (_loginData == null || string.IsNullOrEmpty(_loginData.UserId))
        {
            Debug.Log("bbb");
            StartCoroutine(ShowRegisterRoutine());
        }
        else
        {
            Debug.Log("ccc");
            Auth(_loginData.UserId, _loginData.IsNewAccount);
        }
    }


    private IEnumerator ShowRegisterRoutine()
    {
        yield return new WaitForSeconds(3);
        if (!gameObject.activeSelf)
        {
            Debug.Log(123);
            yield break;
        }

        if (registerHandler == default)
        {
            Debug.Log(456);
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
        Debug.Log(111);
        callBackForOAUTh = _callBack;
        if (Application.isEditor)
        {
        Debug.Log(222);
            FirebaseManager.Instance.TryLoginAndGetData("unity@help.com", "unity123", (_status) =>
            {
        Debug.Log(333);
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
        Debug.Log(444);
        if (!_status)
        {
            Debug.Log(555);
            _callBack?.Invoke(false);
            return;
        }

        if (!CanAuth)
        {
            return;
        }

        Debug.Log(666);
        CanAuth = false;
        JavaScriptManager.Instance.SetUserId(FirebaseManager.Instance.PlayerId);
        Debug.Log(777);
        if (Initialization.Instance==default)
        {
        Debug.Log(888);
            return;
        }
        
        Debug.Log(999);
        Initialization.Instance.CheckForStartingData(_isNewAccount);
    }

    public void AuthFailed()
    {
        callBackForOAUTh?.Invoke(false);
    }
}
