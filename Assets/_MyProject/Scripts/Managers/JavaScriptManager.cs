using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();
    
    [DllImport("__Internal")]
    public static extern void AuthWithFacebook();
    
    [DllImport("__Internal")]
    public static extern void ShowKeyboard();  
    
    [DllImport("__Internal")]
    public static extern void CloseKeyboard();

    public static UnityEvent<string> UpdatedInput;

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
        UpdatedInput?.RemoveAllListeners();
        ShowKeyboard();
    }

    public void HideKeyboard()
    {
        UpdatedInput?.RemoveAllListeners();
        CloseKeyboard();
    }

    public void SetInput(string _key)
    {
        Debug.Log("Got input: "+_key);
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
}
