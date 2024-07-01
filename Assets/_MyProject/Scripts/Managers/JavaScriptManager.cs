using System;
using System.Runtime.InteropServices;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;
    public static Action<string> OnGotUserData;
    public string Version;

    private Action<bool> boundedCallBack;

    [field: SerializeField] public bool IsDemo { get; private set; }

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();

    [DllImport("__Internal")]
    public static extern void AuthLinkingAnonimousUser(string providerName);

    [DllImport("__Internal")]
    public static extern void AuthWithTwitter();    
    
    [DllImport("__Internal")]
    public static extern void AuthWithDiscord();

    [DllImport("__Internal")]
    public static extern void OpenURL(string _url);
    
    [DllImport("__Internal")]
    public static extern void StripePurchaseInit(double _cost);
    
    [DllImport("__Internal")]
    public static extern void DoSetUserId(string _id);    
    
    [DllImport("__Internal")]
    public static extern bool IsPwa();    
    
    [DllImport("__Internal")]
    public static extern string DoCheckIfUserIsLoggedIn();
    
    [DllImport("__Internal")]
    public static extern string DoAnonymousAuth();    
    
    [DllImport("__Internal")]
    public static extern void DoReload();    
    
    [DllImport("__Internal")]
    public static extern void DoSignOut();    
    
    [DllImport("__Internal")]
    public static extern void DoSendMessage(string _roomName, string _message); 
    
    [DllImport("__Internal")]
    public static extern void DoLeaveMatch(); 
    
    [DllImport("__Internal")]
    public static extern void DoMatchMakeAsync(); 
    
    [DllImport("__Internal")]
    public static extern void DoCancelMatchMake(); 
    
    [DllImport("__Internal")]
    public static extern bool DoIsAndroid();    
    
    [DllImport("__Internal")]
    public static extern void DoCheckHasBoundAccount();    
    
    [DllImport("__Internal")]
    public static extern bool CheckIsOnPc();    
    
    [DllImport("__Internal")]
    public static extern bool DoTellDeviceId(string _deviceId);
    public static extern void DoCopyToClipboard(string _text);
    
    [DllImport("__Internal")]
    public static extern void DoCreateAndSetupConnection(string _token);    
    
    [DllImport("__Internal")]
    public static extern void DoJoinFriendlyMatch(string _roomName);


    public bool IsPwaPlatform
    {
        get
        {
            if (Application.isEditor)
            {
                return false;
            }
            
            return IsPwa();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LinkingAnonimousUser(string _providerName)
    {
        AuthLinkingAnonimousUser(_providerName);
    }

    public void GoogleAuth()
    {
        AuthWithGoogle();
    }

    public void CreateAndSetupConnection(string _token)
    {
        DoCreateAndSetupConnection(_token);
    }

    public void LoginTwitter()
    {
        AuthWithTwitter();
    }
    public void LoginWithDiscord()
    {
        AuthWithDiscord();
    }
    
    public void AnonymousAuth()
    {
        DoAnonymousAuth();
    }

    public void StripePurchase(double _cost)
    {
        StripePurchaseInit(_cost);
    }

    public void SetUserId(string _id)
    {
        if (Application.isEditor)
        {
            return;
        }
        
        DoSetUserId(_id);
    }

    public void SignOut()
    {
        DoSignOut();
    }
    
    public void SendMessage(string _roomName, string _message)
    {
        DoSendMessage(_roomName, _message);
    }
    
    public void LeaveMatch()
    {
        DoLeaveMatch();
    }
    
    public void MatchMakeAsync()
    {
        DoMatchMakeAsync();
    }
    
    public void CancelMatchMake()
    {
        DoCancelMatchMake();
    }

    public void CopyToClipboard(string _text)
    {
        if (Application.isEditor)
        {
            GUIUtility.systemCopyBuffer = _text;
            return;
        }

        DoCopyToClipboard(_text);
    }

    public void JoinFriendlyMatch(string _roomName)
    {
        DoJoinFriendlyMatch(_roomName);
    }


    //called from JS 
    public void PurchaseResult(string _resultData)
    {
        PurchaseResponseServer _result = JsonConvert.DeserializeObject<PurchaseResponseServer>(_resultData);
        StripeManager.Instance.PurchaseResult(_result);
    }
    
    public void AuthFinished(string _data)
    {
        Debug.Log("Got user data: " + _data);
        OnGotUserData?.Invoke(_data);
        Debug.Log("Got json from JS: "+ _data);
        if (!AuthHandler.CanAuth)
        {
            Debug.Log("--- Not time for auth");
            return;
        }

        UserLoginData _response = JsonConvert.DeserializeObject<UserLoginData>(_data);
        Debug.Log("Got token: "+_data);
        if (string.IsNullOrEmpty(_data))
        {
            return;
        }

        if (!SceneManager.IsAuthScene)
        {
            Debug.Log("Not auth scene");
            return;
        }

        if (AuthHandler.Instance == default)
        {
            Debug.Log("Auth handler not found");
            return;
        }

        if (_response.Agency is not null)
        {
            Debug.Log("Got agency not null: " + _response.Agency);
            if (AuthHandler.Instance == null)
            {
                Debug.Log("Auth handler is null");
                return;
            }
            
            Debug.Log("Found Auth handler");
            Debug.Log(SceneManager.CurrentSceneName);
            
            AuthHandler.Instance.Auth(_response.UserId,_response.IsNewAccount, _response.IsGuest ,_response.Agency);
        }
        else
        {
            Debug.Log("Got agency null");
            AuthHandler.Instance.Auth(_response.UserId,_response.IsNewAccount,_response.IsGuest);
        }
    }

    public void FailedToAuth()
    {
        AuthHandler.Instance.AuthFailed();
    }

    public void ReloadPage()
    {
        DoReload();
    }

    public void OnUpdatedDiamonds(string _value)
    {
        if (DataManager.Instance == null || DataManager.Instance.PlayerData == default)
        {
            return;
        }
        
        DataManager.Instance.PlayerData.Coins = Convert.ToDouble(_value);
    }

    public void OnUpdatedUSDC(string _value)
    {
        if (DataManager.Instance == null || DataManager.Instance.PlayerData == default)
        {
            return;
        }
        
        Debug.Log("New usdc value: "+_value);
        
        DataManager.Instance.PlayerData.USDC = Convert.ToDouble(_value);
    }

    public void OnUpdatedWalletAddress(string _walletAddress)
    {
        if (DataManager.Instance == null || DataManager.Instance.PlayerData == default)
        {
            return;
        }
        
        DataManager.Instance.PlayerData.UserWalletAddress = _walletAddress;
    }

    public void SuccessLinkingLoginAccount()
    {
        DataManager.Instance.PlayerData.Statistics.NoteCheckPoint("Linked account");
        RegisterAnonymousHandler.Instance.HideRegistrationPage();
    }

    public void UserAlreadyHasAccount() 
    {
        RegisterAnonymousHandler.Instance.UserAlreadyHaveAccount();
    }

    public void RequestUserData()
    {
        DoCheckIfUserIsLoggedIn();
    }

    public bool IsAndroid()
    {
        if (Application.isEditor)
        {
            return true;
        }

        return DoIsAndroid();
    }

    public void CheckHasBoundAccount(Action<bool> _callBack)
    {
        if (Application.isEditor)
        {
            _callBack?.Invoke(true);
            return;
        }
        
        boundedCallBack = _callBack;
        DoCheckHasBoundAccount();
    }

    public void HasBoundedAccount(string _message)
    {
        BoundedData _bounded = JsonConvert.DeserializeObject<BoundedData>(_message);
        boundedCallBack?.Invoke(_bounded.IsBounded);
    }

    public bool IsOnPc()
    {
        if (Application.isEditor)
        {
            return true;
        }
        
        return CheckIsOnPc();
    }

    [SerializeField]private int rankPoints;

    [Button()]
    private void SetRankPoints()
    {
        DataManager.Instance.PlayerData.RankPoints = rankPoints;
        SceneManager.Instance.ReloadScene();
    }
}