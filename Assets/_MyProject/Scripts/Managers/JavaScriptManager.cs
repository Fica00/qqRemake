using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

public class JavaScriptManager : MonoBehaviour
{
    public static JavaScriptManager Instance;

    [DllImport("__Internal")]
    public static extern void AuthWithGoogle();
    
    [DllImport("__Internal")]
    public static extern void AuthWithFacebook();

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
