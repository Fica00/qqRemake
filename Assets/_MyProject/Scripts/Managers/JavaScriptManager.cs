using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;
    
    public const string GAME_LINK = "https://qqweb-b75ae.web.app";

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

    [HideInInspector] public UnityEvent<string> UpdatedInput;

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

    public void DisplayKeyboard()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif
        UpdatedInput?.RemoveAllListeners();
        ShowKeyboard();
    }

    public void HideKeyboard()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
#endif
        UpdatedInput?.RemoveAllListeners();
        CloseKeyboard();
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

    public void LoadURL(string _url)
    {
        OpenURL(_url);
    }
}
