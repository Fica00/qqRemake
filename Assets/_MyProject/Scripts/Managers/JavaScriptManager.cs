using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;
    public static Action<string> OnGotUserData;
    public string Version;

    [field: SerializeField] public bool IsDemo { get; private set; }

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();
    
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
    public static extern void DoCopyToClipboard(string _text);


    public bool ShowPwaWarning
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

    public void GoogleAuth()
    {
        AuthWithGoogle();
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

    public void CopyToClipboard(string _text)
    {
        if (Application.isEditor)
        {
            GUIUtility.systemCopyBuffer = _text;
            return;
        }

        DoCopyToClipboard(_text);
    }


    //called from JS 
    public void PurchaseResult(string _resultData)
    {
        PurchaseResponseServer _result = JsonConvert.DeserializeObject<PurchaseResponseServer>(_resultData);
        StripeManager.Instance.PurchaseResult(_result);
    }
    
    public void AuthFinished(string _data)
    {
        Debug.Log("Got user data: "+_data);
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
        
        AuthHandler.Instance.Auth(_response.UserId,_response.IsNewAccount);
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

    public void RequestUserData()
    {
        DoCheckIfUserIsLoggedIn();
    }
}