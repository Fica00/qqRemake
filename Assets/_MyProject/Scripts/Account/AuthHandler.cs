using System;
using System.Collections;
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
        Debug.Log("--- Requesting authentication data");
        UserLoginData _userData = JavaScriptManager.Instance.GetUserData();
        if (_userData == null || string.IsNullOrEmpty(_userData.UserId))
        {
            StartCoroutine(ShowRegisterRoutine());
        }
        else
        {
            Auth(_userData.UserId);
        }
    }

    private IEnumerator ShowRegisterRoutine()
    {
        yield return new WaitForSeconds(3);
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
                HandleLoginResult(_status,callBackForOAUTh);
            });
        }
        else
        {
            JavaScriptManager.Instance.AnonymousAuth();
        }
    }

    public void Auth(string _id)
    {
        FirebaseManager.Instance.SignIn(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh);
        });
    }

    private void HandleLoginResult(bool _status, Action<bool> _callBack)
    {
        if (!_status)
        {
            _callBack?.Invoke(false);
            return;
        }

        CanAuth = false;
        JavaScriptManager.Instance.SetUserId(FirebaseManager.Instance.PlayerId);
        Initialization.Instance.CheckForStartingData();
    }

    public void AuthFailed()
    {
        callBackForOAUTh?.Invoke(false);
    }
}
