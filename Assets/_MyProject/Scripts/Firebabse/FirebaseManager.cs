using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private const string WEB_API_KEY = "AIzaSyBwAcfu-00sN9qH66i499dzf5SjBaFQ358";

    private string userLocalId;
    private string userIdToken;
    private string projectLink = "https://qqweb-b75ae-default-rtdb.firebaseio.com";
    public string UserDataLink => $"{projectLink}/users/{userLocalId}/";
    public string UsersLink => $"{projectLink}/users/";
    private string GameDataLink => $"{projectLink}/gameData/";
    private string DeckStatistic => $"{GameDataLink}/DeckStatistic/";
    private string WithdrawalsLink => $"{projectLink}/withdrawals/";
    private string ReportLink => $"{projectLink}/reports/";
    private string QommonStatistic => $"{projectLink}/gameData/QommonStatistic/";
    private string MarketPlaceLink => $"{projectLink}/gameData/{nameof(DataManager.Instance.GameData.Marketplace)}/";

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
                _callBack?.Invoke(true);
            },
            (_result) =>
            {
                _callBack?.Invoke(false);
            }));
    }

    private void CollectGameData(Action<bool> _callBack)
    {
        StartCoroutine(Get(GameDataLink + ".json", (_result) =>
        {
            DataManager.Instance.SetGameData(_result);
            CollectPlayerData(_callBack);
        }, (_) => { _callBack?.Invoke(false); }));
    }

    //private void CollectPlayerData(Action<bool> _callBack)
    //{
    //    StartCoroutine(Get(UserDataLink + "/.json", (_result) =>
    //    {
    //        DataManager.Instance.SetPlayerData(_result);
    //        _callBack?.Invoke(true);
    //    }, (_) => { _callBack?.Invoke(false); }));
    //}

    private void CollectPlayerData(Action<bool> _callBack)
    {
        StartCoroutine(CollectPlayerDataCoroutine(_callBack));
    }

    public void SignIn(string _firebaseId, Action<bool> _callBack)
    {
        userLocalId = _firebaseId;
        userIdToken = string.Empty;
        CollectGameData(_callBack);
    }

    public void SignOut()
    {
        userLocalId = string.Empty;
    }

    public void SaveValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";
        StartCoroutine(Patch(UserDataLink + ".json", _valueString, (_result) =>
        {
            
        }, (_result) =>
        {
            DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(JavaScriptManager.Instance.ReloadPage);
            DialogsManager.Instance.OkDialog.Setup("Failed to interact with database, we copied the error to your clipboard please forward it to us on Discord");
            JavaScriptManager.Instance.CopyToClipboard(_result);
        }));
    }
    
    public void SaveString(string _path, string _value)
    {
        string _valueString = "{\"" + _path + "\":\"" + _value + "\"}";
        StartCoroutine(Patch(UserDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(JavaScriptManager.Instance.ReloadPage);
            DialogsManager.Instance.OkDialog.Setup("Failed to interact with database, we copied the error to your clipboard please forward it to us on Discord");
            JavaScriptManager.Instance.CopyToClipboard(_result);
        }));
    }

    public void RemoveGamePassFromMarketplace(GamePassOffer _offer, Action<bool> _callBack)
    {
        GameObject _loading = Instantiate(AssetsManager.Instance.Loading, null);
        GetMarketplace((_marketPlaceData) =>
        {
            var _items = JsonConvert.DeserializeObject<Dictionary<string,GamePassOffer>>(_marketPlaceData);
            string _keyToRemove = default;
            KeyValuePair<string, GamePassOffer> _offerInMarketplace = GameData.GetMarketplaceOffer(_offer, _items);
            if (_offerInMarketplace.Key == default || _offerInMarketplace.Value == default)
            {
                _callBack?.Invoke(false);
                return;
            }

            _keyToRemove = _offerInMarketplace.Key;

            if (!string.IsNullOrEmpty(_keyToRemove))
            {
                string _itemURL = $"{MarketPlaceLink}{_keyToRemove}.json";
                StartCoroutine(Delete(_itemURL, (_result) =>
                {
                    _callBack?.Invoke(true);
                    Destroy(_loading);
                }, (_result) =>
                {
                    _callBack?.Invoke(false);
                    Destroy(_loading);
                }));
            }
            else
            {
                _callBack?.Invoke(false);
                Destroy(_loading);
            }
            
        });
    }

    public void AddUSDCToPlayer(double _amount, string _receiverId, Action<bool> _callBack)
    {
        string _key = nameof(DataManager.Instance.PlayerData.USDC);
        string _url = UsersLink + _receiverId;
        StartCoroutine(Get(_url+"/"+_key + "/.json",
            (_result) =>
            {
                double _coins = 0;
                if (!string.IsNullOrEmpty(_result))
                {
                    _coins = JsonConvert.DeserializeObject<double>(_result);
                }

                _coins += _amount;
                string _jsonData = "{\""+_key+"\": " + _coins + "}";
                StartCoroutine(Patch(_url+"/.json", _jsonData, (_result) =>
                {
                    _callBack?.Invoke(true);
                }, (_error) =>
                {
                    _callBack?.Invoke(false);
                }));
            }, (_error) =>
            {
                _callBack?.Invoke(false);
            }));
    }

    public void RefreshMarketplace(Action _callBack)
    {
        GameObject _loading = Instantiate(AssetsManager.Instance.Loading);
        GetMarketplace(OnLoadedMarketplace);

        void OnLoadedMarketplace(string _data)
        {
            DataManager.Instance.GameData.Marketplace = JsonConvert.DeserializeObject<Dictionary<string,GamePassOffer>>
            (_data);
            _callBack?.Invoke();
            Destroy(_loading);
        }
    }

    private void GetMarketplace(Action<string> _callBack)
    {
        StartCoroutine(Get(MarketPlaceLink+".json", (_result) =>
        {
            _callBack?.Invoke(_result);
        }, (_result) => { _callBack?.Invoke(null); }));
    }

    public void AddOfferToMarketplace(GamePassOffer _offer, Action<bool> _callBack)
    {
        string _key = Guid.NewGuid().ToString();
        string _dataJson = JsonConvert.SerializeObject(_offer);
        string _url = MarketPlaceLink + _key+"/.json";
        StartCoroutine(Patch(_url, _dataJson, (_result) =>
        {
            DataManager.Instance.GameData.Marketplace.Add(_key,_offer);
            _callBack?.Invoke(true);
        }, (_error) =>
        {
            Debug.Log(_error);
            _callBack?.Invoke(false);
        }));
    }

    public void UpdateCardsWinLoseCount(List<int> _qommons, bool _didIWin)
    {
        string _sectionKey = _didIWin ? "win" : "lose";
        _qommons = _qommons.OrderBy(_x => _x).ToList();
        string _url = DeckStatistic + string.Join("-", _qommons);
        _url += "/" + _sectionKey;
        Debug.Log(_url);
        string _valueString = "{\"date\":\"" +DateTime.UtcNow  + "\"}";
        StartCoroutine(Patch(_url+"/"+Guid.NewGuid()+"/" +".json", _valueString, _stringValue =>
        {
        }, _ =>
        {
        }));
    }

    public void RequestWithdrawal(WithdrawalData _data, Action<string> _onSuccess, Action<string> _onFailed)
    {
        string _url = WithdrawalsLink + _data.WithdrawalId+".json";
        StartCoroutine(Patch(_url, JsonConvert.SerializeObject(_data), _onSuccess, _onFailed));
    }

    public void ReportBug(ReportDate _reportData, Action<string> _onSuccess, Action<string> _onFailed) {
        string _url = ReportLink + Guid.NewGuid().ToString()+ ".json";
        StartCoroutine(Patch(_url, JsonConvert.SerializeObject(_reportData), _onSuccess, _onFailed));
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
                Debug.Log("FAILED: "+_webRequest.error);
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }

    public IEnumerator Patch(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
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
                _onError?.Invoke(_webRequest.error + "\n"+ JsonConvert.SerializeObject(_webRequest));
                Debug.Log("Failed to save: "+_jsonData);
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
    private IEnumerator CollectPlayerDataCoroutine(Action<bool> _callBack)
    {
        yield return StartCoroutine(Get(UserDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
            _callBack?.Invoke(true);
        }, (_) => { _callBack?.Invoke(false); }));



        if (DataManager.Instance.PlayerData is null)
        {
            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(Get(UserDataLink + "/.json", (_result) =>
            {
                DataManager.Instance.SetPlayerData(_result);
                _callBack?.Invoke(true);
            }, (_) => { _callBack?.Invoke(false); }));
        }
    }

}
