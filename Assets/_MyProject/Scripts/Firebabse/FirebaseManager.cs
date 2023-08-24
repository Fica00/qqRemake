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
                DataManager.Instance.CreatePlayerDataEmpty();
                SetStartingData(_callBack);
            }, (_result) =>
            {
                Debug.Log("Register failed");
                _callBack?.Invoke(false);
            }));
    }

    private void SetStartingData(Action<bool> _callBack)
    {
        Debug.Log("Entering starting data");
        string _data = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
        StartCoroutine(Put(userDataLink + "/.json", _data, (_result) =>
            {
                Debug.Log("Entered starting data sucess");
                CollectGameData(_callBack);
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
            CollectPlayerData(_callBack);
        }, (_result) => { _callBack?.Invoke(false); }));
    }

    private void CollectPlayerData(Action<bool> _callBack)
    {
        Debug.Log("Setting player data");
        StartCoroutine(Get(userDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
            _callBack?.Invoke(true);
        }, (_result) => { _callBack?.Invoke(false); }));
    }

    public void SignInWithGoogle(string _googleIdToken)
    {
        StartCoroutine(SignInCoroutine(_googleIdToken));
    }

    private IEnumerator SignInCoroutine(string _googleIdToken)
    {
        WWWForm form = new WWWForm();
        form.AddField("postBody", "id_token=" + _googleIdToken + "&providerId=google.com");
        form.AddField("requestUri", "http://localhost");
        form.AddField("returnIdpCredential", "true");
        form.AddField("returnSecureToken", "true");

        UnityWebRequest www = UnityWebRequest.Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key="+WEB_API_KEY, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("Received: " + www.downloadHandler.text);
        }
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
