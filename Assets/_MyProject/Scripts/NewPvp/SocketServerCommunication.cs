using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class SocketServerCommunication : MonoBehaviour
{
    public static Action OnOpponentJoinedRoom;
    public static Action OnOpponentLeftRoom;
    public static Action OnILeftRoom;
    public static Action OnRoomIsFull;
    public static Action MatchMakingStarted;

    public static Action<bool> OnInitFinished;
    
    public static SocketServerCommunication Instance;
    private const string SERVER_URI = "https://api.qoomonquest.com/hubs/game";
    private HubConnection connection;
    private string authKey;

    public MatchData MatchData { get; private set; }

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetAuthToken(string _authKey)
    {
        authKey = _authKey;
    }
    
    public void ReceiveConnectionOutcome(int _success)
    {
        OnInitFinished?.Invoke(_success == 1);
    }

    public async void Init()
    {
        if (!Application.isEditor)
        {
            JavaScriptManager.Instance.CreateAndSetupConnection(authKey);
            return;
        }
        
        Debug.Log($"Creating connection");

        if (connection!=null)
        {
            OnInitFinished?.Invoke(true);
            return;
        }

        connection = new HubConnectionBuilder()
            .WithUrl(SERVER_URI
                , _options =>
                {
                    _options.AccessTokenProvider = () => Task.FromResult(authKey);
                    _options.SkipNegotiation = true;
                    _options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                }
                )
            .WithAutomaticReconnect()
            .Build();
        
        Debug.Log($"Setting callbacks");

        connection.On<string>(nameof(UserDisconnectedAsync), UserDisconnectedAsync);        
        connection.On<string>(nameof(UserRejoinedAsync), UserRejoinedAsync);
        connection.On<string>(nameof(UserLeftAsync), UserLeftAsync);
        connection.On<List<string>>(nameof(ReceiveOldMessagesAsync), ReceiveOldMessagesAsync);
        connection.On<string>(nameof(ReceiveMessageAsync), ReceiveMessageAsync);   
        connection.On<bool>(nameof(MatchLeftAsync), MatchLeftAsync);   
        connection.On<string,string,string>(nameof(MatchFoundAsync), MatchFoundAsync);
        connection.On(nameof(MatchMakingStartedAsync),MatchMakingStartedAsync);
        connection.On(nameof(MatchMakingCanceledAsync),MatchMakingCanceledAsync);
        connection.On(nameof(RoomIsFullAsync),RoomIsFullAsync);

        Debug.Log($"Trying to connect");
        try
        {
            await connection.StartAsync();
            Debug.Log($"Connection successful");
            OnInitFinished?.Invoke(true);
        }
        catch (Exception _error)
        {
            Debug.Log($"Trying to connect with: {SERVER_URI} , authToken: {authKey}\nFailed with error: {_error}");
            OnInitFinished?.Invoke(false);
        }
    }

    #region Receive messages

    private void UserDisconnectedAsync(string _userName)
    {
        Debug.Log("UserDisconnectedAsync: "+_userName);
        OnOpponentLeftRoom?.Invoke();
    }
    
    private void MatchMakingCanceledAsync()
    {
        Debug.Log("MatchMakingCanceledAsync: ");
        OnILeftRoom?.Invoke();
    }
    
    private void UserLeftAsync(string _userName)
    {
        Debug.Log("UserDisconnectedAsync: "+_userName);
        OnOpponentLeftRoom?.Invoke();
    }

    private void UserRejoinedAsync(string _userName)
    {
        Debug.Log("UserRejoinedAsync: "+_userName);
    }
    
    private void MatchLeftAsync(bool _status)
    {
        if (!_status)
        {
            Debug.Log("Failed to leave room");
            return;
        }

        OnILeftRoom?.Invoke();
    }
    
    private void MatchLeftAsyncFromJs(int _status)
    {
        MatchLeftAsync(_status==1);
    }

    private void ReceiveOldMessagesAsync(List<string> _messages)
    {
        Debug.Log("ReceiveOldMessagesAsync: ");
        foreach (var _message in _messages)
        {
            Debug.Log(_message);
        }
    }

    private void ReceiveOldMessagesAsyncJson(string _jsonData)
    {
        Debug.Log("ReceiveOldMessagesAsync json call" );
        List<string> _messages = JsonConvert.DeserializeObject<List<string>>(_jsonData);
        ReceiveOldMessagesAsync(_messages);
    }
    
    private void ReceiveMessageAsync(string _message)
    {
        Debug.Log("ReceiveMessageAsync: "+_message);
        ExecuteMessage(JsonConvert.DeserializeObject<MessageData>(_message));
    }

    private void MatchFoundAsync(string _roomName, string _firstPlayer, string _secondPlayer)
    {
        Debug.Log($"MatchFoundAsync: {_firstPlayer} vs {_secondPlayer}");
        MatchData = new MatchData() { RoomName = _roomName,Players = new List<string>() { _firstPlayer, _secondPlayer } };
        OnOpponentJoinedRoom?.Invoke();
    }

    private void MatchFoundAsyncJson(string _jsonData)
    {
        Debug.Log($"MatchFoundAsync json call: {_jsonData}");
        MatchFoundData _data = JsonConvert.DeserializeObject<MatchFoundData>(_jsonData);
        MatchFoundAsync(_data.RoomName, _data.FirstPlayer, _data.SecondPlayer);
    }
    
    private void MatchMakingStartedAsync()
    {
        Debug.Log($"MatchMakingStartedAsync");
        MatchMakingStarted?.Invoke();
    }

    private void RoomIsFullAsync()
    {
        OnRoomIsFull?.Invoke();
    }
    
    #endregion

    #region Send messages

    public new void SendMessage(string _message)
    {
        if (!Application.isEditor)
        {
            Debug.Log("SendMessage not editor: " + _message);
            JavaScriptManager.Instance.SendMessage(MatchData.RoomName,_message);
            return;
        }
        
        Debug.Log("SendMessage editor: " + _message);
        connection.SendAsync("SendMessageAsync", MatchData.RoomName,_message);
    }

    public void StartMatchMaking()
    {
        if (!Application.isEditor)
        {
            Debug.Log("StartMatchMaking not editor");
            JavaScriptManager.Instance.MatchMakeAsync();
            return;
        }
        
        Debug.Log("StartMatchMaking editor");
        connection.SendAsync("MatchMakeAsync");
    }

    public void LeaveRoom()
    {
        if (!Application.isEditor)
        {
            Debug.Log("LeaveRoom not editor");
            JavaScriptManager.Instance.LeaveMatch();
            return;
        }
        
        Debug.Log("LeaveRoom editor");
        connection.SendAsync("LeaveMatchAsync");
    }

    public void CancelMatchMaking()
    {
        if (!Application.isEditor)
        {
            Debug.Log("CancelMatchMaking not editor");
            JavaScriptManager.Instance.CancelMatchMake();
            return;
        }
        
        Debug.Log("CancelMatchMaking editor");
        connection.SendAsync("CancelMatchMakeAsync");
    }

    public void JoinFriendlyMatch(string _name)
    {
        if (!Application.isEditor)
        {
            Debug.Log("CancelMatchMaking not editor");
            JavaScriptManager.Instance.JoinFriendlyMatch(_name);
            return;
        }
        
        connection.SendAsync("JoinFriendlyMatch", _name);
    }
    
    #endregion

    public void RegisterMessage(GameObject _object, string _functionName, string _data= null)
    {
        MessageData _message = new MessageData { GameObjectName = _object.name, MethodName = _functionName, Data = _data };
        string _messageJson = JsonConvert.SerializeObject(_message);
        SendMessage(_messageJson);
    }

    private void ExecuteMessage(MessageData _messageData)
    {
        string _objectName = _messageData.GameObjectName;
        GameObject _targetObject = GameObject.Find(_objectName);
        if (_targetObject==null)
        {
            Debug.LogError($"GameObject {_objectName} not found.");
        }
        
        MonoBehaviour _target = _targetObject.GetComponent<MonoBehaviour>();
        if (_target == null)
        {
            Debug.LogError($"No MonoBehaviour found on object {_objectName}.");
        }

        string _methodName = _messageData.MethodName;
        
        try
        {
            _target.SendMessage(_methodName,_messageData.Data);
        }
        catch (Exception _e)
        {
            Debug.LogError($"Failed to call {_methodName} on {_objectName}: "+_e);
            throw;
        }
    }

    public void ResetMatchData()
    {
        MatchData = null;
    }
}