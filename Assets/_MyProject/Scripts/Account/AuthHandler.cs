using System;
using UnityEngine;

public class AuthHandler : MonoBehaviour
{
    private const string AUTH_METHOD = "AuthMethod";
    private const string AUTH_PARMS = "AuthParms";
    public static AuthHandler Instance;

    [SerializeField] private RegisterHandler registerHandler;

    private Action callBack;
    private Action<bool> callBackForOAUTh;
    private AuthMethod currentAuthMethod;
    private string currentAuthParms;

    private void Awake()
    {
        Instance = this;
    }

    public void Authenticate(Action _callBack)
    {
        callBack = _callBack;
        int _authMethod = PlayerPrefs.GetInt(AUTH_METHOD, -1);
        if (_authMethod==-1)
        {
            registerHandler.Setup();
        }
        else if (_authMethod == (int) AuthMethod.Email)
        {
            string _authParms = PlayerPrefs.GetString(AUTH_PARMS);
            string _email = String.Empty;
            string _password = String.Empty;
            bool _foundFirstComma = false;
            for (int _i = 0; _i < _authParms.Length; _i++)
            {
                if (_authParms[_i]==',' && !_foundFirstComma)
                {
                    _foundFirstComma = true;
                    continue;
                }

                if (_foundFirstComma)
                {
                    _password += _authParms[_i];
                }
                else
                {
                    _email += _authParms[_i];
                }
            }
            
            LoginWithEmail(_email,_password,(_status) =>
            {
                if (_status)
                {
                    return;
                }
                
                registerHandler.Setup();
                PlayerPrefs.DeleteAll();
            });
        }
    }

    public void LoginWithFacebook(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        currentAuthMethod = AuthMethod.Facebook;
        JavaScriptManager.Instance.FacebookAuth();
    }

    public void LoginWithGoogle(Action<bool> _callBack)
    {
        callBackForOAUTh = _callBack;
        currentAuthMethod = AuthMethod.Google;
        JavaScriptManager.Instance.GoogleAuth();
    }

    public void AuthWithGoogle(string _id)
    {
        FirebaseManager.Instance.SignInWithGoogle(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh);
        });
    }

    public void AuthWithFacebook(string _id)
    {
        FirebaseManager.Instance.SignInWithFacebook(_id, (_status) =>
        {
            HandleLoginResult(_status,callBackForOAUTh);
        });
    }

    public void LoginWithEmail(string _email, string _password, Action<bool> _callBack)
    {
        currentAuthMethod = AuthMethod.Email;
        currentAuthParms = _email+","+_password;
        FirebaseManager.Instance.TryLoginAndGetData(_email, _password, (_status) =>
        {
            HandleLoginResult(_status,_callBack);
        });
    }


    public void HandleLoginResult(bool _status, Action<bool> _callBack)
    {
        if (!_status)
        {
            _callBack?.Invoke(false);
            return;
        }
        PlayerPrefs.SetInt(AUTH_METHOD, (int)currentAuthMethod);
        PlayerPrefs.SetString(AUTH_PARMS, currentAuthParms);
        callBack?.Invoke();
    }
}
