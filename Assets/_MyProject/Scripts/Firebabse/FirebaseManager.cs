using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private const string WEB_API_KEY = "AIzaSyBwAcfu-00sN9qH66i499dzf5SjBaFQ358";

    private string userLocalId;
    private string userIdToken;
    private string projectLink = "https://qqweb-b75ae-default-rtdb.firebaseio.com/";
    private string userDataLink => $"{projectLink}/users/{userLocalId}/";
    private string gameDataLink => $"{projectLink}/gameData/";

    public string PlayerId => userLocalId;

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

    public void TryLoginAndGetData(string _email, string _passwrod, Action<bool> _callBack)
    {
        string _loginParms = "{\"email\":\"" + _email + "\",\"password\":\"" + _passwrod +
                             "\",\"returnSecureToken\":true}";

        Debug.Log(_loginParms);
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY,
            _loginParms, (_result) =>
            {
                SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
                userIdToken = _signInResponse.IdToken;
                userLocalId = _signInResponse.LocalId;
                CollectGameData(_callBack);
            }, (_result) => { Register(_callBack, _loginParms); }, false));
    }

    private void Register(Action<bool> _callBack, string _parms)
    {
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _parms,
            (_result) =>
            {
                RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
                userIdToken = _registerResult.IdToken;
                userLocalId = _registerResult.LocalId;
            }, (_result) =>
            {
                Debug.Log("Register failed");
                _callBack?.Invoke(false);
            }));
    }

    public void SetStartingData(Action<bool> _callBack)
    {
        Debug.Log("Entering starting data");
        DataManager.Instance.CreatePlayerDataEmpty();
        string _data = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
        StartCoroutine(Put(userDataLink + "/.json", _data, (_result) =>
            {
                Debug.Log("Entered starting data sucess");
                _callBack?.Invoke(true);
            },
            (_result) =>
            {
                Debug.Log("Entered starting data failed");
                _callBack?.Invoke(false);
            }));
    }

    private void CollectGameData(Action<bool> _callBack)
    {
        Debug.Log("collecting game data");
        StartCoroutine(Get(gameDataLink + ".json", (_result) =>
        {
            DataManager.Instance.SetGameData(_result);
            Debug.Log(JsonConvert.SerializeObject(_result));
            CollectPlayerData(_callBack);
        }, (_result) => { _callBack?.Invoke(false); }));
    }

    private void CollectPlayerData(Action<bool> _callBack)
    {
        StartCoroutine(Get(userDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
            Debug.Log(JsonConvert.SerializeObject(_result));
            _callBack?.Invoke(true);
        }, (_result) => { _callBack?.Invoke(false); }));
    }

    public void SignInWithGoogle(string _firebaseId, Action<bool> _callBack)
    {
        userLocalId = _firebaseId;
        userIdToken = string.Empty; //todo if something isn't working add me
        PlayerPrefs.SetInt(AuthHandler.AUTH_METHOD,(int) AuthMethod.Google);
        CollectGameData(_callBack);
    }

    public void SignInWithFacebook(string _firebaseId, Action<bool> _callBack)
    {
        userLocalId = _firebaseId;
        userIdToken = string.Empty; //todo if something isn't working add me
        PlayerPrefs.SetInt(AuthHandler.AUTH_METHOD,(int) AuthMethod.Facebook);
        CollectGameData(_callBack);
    }

    private IEnumerator Get(string uri, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.Dispose();
        }
    }

    private IEnumerator Post(string uri, string jsonData, Action<string> onSuccess, Action<string> onError,
        bool _includeHeader = true)
    {
        if (userIdToken != null)
        {
            if (_includeHeader)
            {
                uri = $"{uri}?auth={userIdToken}";
            }
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    private IEnumerator Put(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        // If the userIdToken is available, append it to the URI
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    private IEnumerator Patch(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "PATCH"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();


            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    private IEnumerator Delete(string uri, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }
}
