using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;

    [field: SerializeField] public bool IsDemo { get; private set; }

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();
    
    [DllImport("__Internal")]
    public static extern void AuthWithFacebook();

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

    public void FacebookAuth()
    {
        AuthWithFacebook();
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


    //called from JS 
    public void PurchaseResult(string _resultData)
    {
        PurchaseResponseServer _result = JsonConvert.DeserializeObject<PurchaseResponseServer>(_resultData);
        StripeManager.Instance.PurchaseResult(_result);
    }
    
    public void AuthFinished(string _data)
    {
        if (AuthHandler.IsAuthenticated)
        {
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
            return;
        }

        if (AuthHandler.Instance == default)
        {
            return;
        }
        
        AuthHandler.Instance.Auth(_response.UserId);
    }

    public void FailedToAuth()
    {
        AuthHandler.Instance.AuthFailed();
    }

    public void ReloadPage()
    {
        DoReload();
    }

    public void OnUpdatedDiamonds(double _value)
    {
        if (DataManager.Instance == null || DataManager.Instance.PlayerData == default)
        {
            return;
        }
        
        DataManager.Instance.PlayerData.Coins = _value;
    }

    public void OnUpdatedUSDC(double _value)
    {
        if (DataManager.Instance == null || DataManager.Instance.PlayerData == default)
        {
            return;
        }
        
        DataManager.Instance.PlayerData.USDC = _value;
    }

    public UserLoginData GetUserData()
    {
        if (Application.isEditor)
        {
            return null;
        }
        string _userData = DoCheckIfUserIsLoggedIn();
        if (string.IsNullOrEmpty(_userData))
        {
            return null;
        }
        Debug.Log("Got data from JS: "+_userData);
        UserLoginData _userLoginData = JsonConvert.DeserializeObject<UserLoginData>(_userData);

        return _userLoginData;
    }
}