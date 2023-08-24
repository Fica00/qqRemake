using System.Runtime.InteropServices;
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
        Debug.Log($"Received answer for Google auth from JS side: "+_data);
        //AuthHandler.Instance.AuthWithGoogle(_id);
    }

    public void AuthWithFacebook(string _data)
    {
        Debug.Log($"Received answer for Facebook auth from JS side: "+_data);
    }
}
