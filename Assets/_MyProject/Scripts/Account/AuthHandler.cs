using System;
using UnityEngine;

public class AuthHandler : MonoBehaviour
{
    public static AuthHandler Instance;
    [SerializeField] private RegisterHandler registerHandler;
    private Action<bool> callBackForOAUTh;

    private void Awake()
    {
        Instance = this;
    }

    public void Authenticate()
    {
        UserLoginData _userData = JavaScriptManager.Instance.GetUserData();
        if (_userData == null || string.IsNullOrEmpty(_userData.UserId))
        {
            registerHandler.Setup();
        }
        else
        {
            Auth(_userData.UserId);
        }
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
        JavaScriptManager.Instance.AnonymousAuth();
    }

    public void Auth(string _id)
    {
        FirebaseManager.Instance.SignIn(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh);
        });
    }

    public void HandleLoginResult(bool _status, Action<bool> _callBack)
    {
        if (!_status)
        {
            Debug.Log("Failed to login????");
            _callBack?.Invoke(false);
            return;
        }
        
        JavaScriptManager.Instance.SetUserId(FirebaseManager.Instance.PlayerId);
        Initialization.Instance.CheckForStartingData();
    }

    public void AuthFailed()
    {
        callBackForOAUTh?.Invoke(false);
    }
}
