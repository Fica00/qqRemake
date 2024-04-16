using System.Runtime.InteropServices;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;

    [field: SerializeField] public bool IsDemo { get; private set; }
    public string GameLink => IsDemo? "https://qq-remake-mauve.vercel.app/": "https://qq-remake-development.vercel.app/";

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();
    
    [DllImport("__Internal")]
    public static extern void AuthWithFacebook();
    
    [DllImport("__Internal")]
    public static extern void ShowKeyboard();  
    
    [DllImport("__Internal")]
    public static extern void CloseKeyboard();
    
    [DllImport("__Internal")]
    public static extern void OpenURL(string _url);
    
    [DllImport("__Internal")]
    public static extern void StripePurchaseInit(double _cost);
    
    [DllImport("__Internal")]
    public static extern void DoSetUserId(string _id);    
    
    [DllImport("__Internal")]
    public static extern bool IsPwa();


    [HideInInspector] public UnityEvent<string> UpdatedInput;

    public bool IsBrowser
    {
        get
        {
            if (Application.isEditor)
            {
                return false;
            }
            
            return !IsPwa();
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

    public void PurchaseResult(string _resultData)
    {
        PurchaseResponseServer _result = JsonConvert.DeserializeObject<PurchaseResponseServer>(_resultData);
        
        StripeManager.Instance.PurchaseResult(_result);
    }
    
    public void SetInput(string _key)
    {
        UpdatedInput?.Invoke(_key);
    }

    public void AuthWithGoogle(string _data)
    {
        PlayerPrefs.SetInt(AuthHandler.AUTH_METHOD, (int)AuthMethod.Google);
        FirebaseAuthResponse _response = JsonConvert.DeserializeObject<FirebaseAuthResponse>(_data);
        Debug.Log("Got token: "+_data);
        AuthHandler.Instance.AuthWithGoogle(_response.Id);
    }

    public void AuthWithFacebook(string _data)
    {
        PlayerPrefs.SetInt(AuthHandler.AUTH_METHOD, (int)AuthMethod.Facebook);
        FirebaseAuthResponse _response = JsonConvert.DeserializeObject<FirebaseAuthResponse>(_data);
        Debug.Log("Got token: "+_data);
        AuthHandler.Instance.AuthWithFacebook(_response.Id);
    }

    public void FailedToAuth()
    {
        AuthHandler.Instance.AuthFailed();
    }

    public void LoadURL(string _url)
    {
        OpenURL(_url);
    }

    public void OnUpdatedDiamonds(double _value)
    {
        DataManager.Instance.PlayerData.Coins = _value;
    }

    public void OnUpdatedUSDC(double _value)
    {
        DataManager.Instance.PlayerData.USDC = _value;
    }

    [Button()]
    private void ReloadScene()
    {
        SceneManager.Instance.ReloadScene();
    }
}