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
    public string UserDataLink => $"{projectLink}/users/{userLocalId}/";
    private string GameDataLink => $"{projectLink}/gameData/";

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
                CollectGameData(_callBack);
            }, (_result) =>
            {
                Debug.Log("Register failed");
                _callBack?.Invoke(false);
            }));
    }

    public void SetStartingData(Action<bool> _callBack)
    {
        DataManager.Instance.CreatePlayerDataEmpty();
        string _data = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
        StartCoroutine(Put(UserDataLink + "/.json", _data, (_result) =>
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
        StartCoroutine(Get(GameDataLink + ".json", (_result) =>
        {
            DataManager.Instance.SetGameData(_result);
            CollectPlayerData(_callBack);
        }, (_result) => { _callBack?.Invoke(false); }));
    }

    private void CollectPlayerData(Action<bool> _callBack)
    {
        StartCoroutine(Get(UserDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
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

    public void SaveValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";
        StartCoroutine(Patch(UserDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log(_valueString);
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }
    
    public void SaveString(string _path, string _value)
    {
        string _valueString = "{\"" + _path + "\":\"" + _value + "\"}";
        StartCoroutine(Patch(UserDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }

    private IEnumerator Get(string _uri, Action<string> _onSuccess, Action<string> _onError)
    {
        if (userIdToken != null)
        {
            _uri = $"{_uri}?auth={userIdToken}";
        }
        
        using (UnityWebRequest _webRequest = UnityWebRequest.Get(_uri))
        {
            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(_webRequest.error);
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.Dispose();
        }
    }

    private IEnumerator Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError,
        bool _includeHeader = true)
    {
        if (userIdToken != null)
        {
            if (_includeHeader)
            {
                _uri = $"{_uri}?auth={userIdToken}";
            }
        }

        using (UnityWebRequest _webRequest = UnityWebRequest.Post(_uri, _jsonData))
        {
            byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
            _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
            _webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }

    private IEnumerator Put(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        // If the userIdToken is available, append it to the URI
        if (userIdToken != null)
        {
            _uri = $"{_uri}?auth={userIdToken}";
        }

        using (UnityWebRequest _webRequest = UnityWebRequest.Put(_uri, _jsonData))
        {
            byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
            _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
            _webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(_webRequest.error);
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }

    private IEnumerator Patch(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        if (userIdToken != null)
        {
            _uri = $"{_uri}?auth={userIdToken}";
        }

        using (UnityWebRequest _webRequest = new UnityWebRequest(_uri, "PATCH"))
        {
            byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
            _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
            _webRequest.downloadHandler = new DownloadHandlerBuffer();


            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }

    private IEnumerator Delete(string _uri, Action<string> _onSuccess, Action<string> _onError)
    {
        if (userIdToken != null)
        {
            _uri = $"{_uri}?auth={userIdToken}";
        }

        using (UnityWebRequest _webRequest = UnityWebRequest.Delete(_uri))
        {
            _webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }
}
