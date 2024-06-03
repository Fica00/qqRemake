using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class HttpCommunicationHandler : MonoBehaviour
{
    public static HttpCommunicationHandler Instance;
    private const string SERVER_URI = "http://ec2-54-234-153-167.compute-1.amazonaws.com/";
    private string AuthenticateUri => SERVER_URI + "authenticate";

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

    public void Authenticate(string _firebaseId, Action<bool, string> _callBack)
    {
        string _jsonData = JsonConvert.SerializeObject(new { userId = _firebaseId });
        StartCoroutine(Post(AuthenticateUri, _jsonData, _data =>
            {
                _callBack?.Invoke(true, _data);
            },
            _result => { _callBack?.Invoke(false, _result); }));
    }

    private IEnumerator Get(string _uri, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Get(_uri);
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

    private IEnumerator Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Post(_uri, _jsonData);
        byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();
        _webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(_webRequest.error);
            Debug.Log(_webRequest.downloadHandler.text);
            Debug.Log(_webRequest.downloadHandler.data);
            _onError?.Invoke(_webRequest.error);
        }

        _webRequest.uploadHandler.Dispose();
        _webRequest.downloadHandler.Dispose();
        _webRequest.Dispose();
    }

    private IEnumerator Put(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Put(_uri, _jsonData);
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

    private IEnumerator Patch(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = new UnityWebRequest(_uri, "PATCH");
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

    private IEnumerator Delete(string _uri, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Delete(_uri);
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