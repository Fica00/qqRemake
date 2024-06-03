using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class SocketServerCommunication : MonoBehaviour
{
    public static Action OnILeftRoom;
    public static Action OnOpponentJoinedRoom;
    public static Action OnOpponentLeftRoom;
    
    public static SocketServerCommunication Instance;
    private const string SERVER_URI = "https://ec2-54-234-153-167.compute-1.amazonaws.com/hubs/game";
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

    public async void Init(Action<bool> _callBack)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(SERVER_URI
                , _options =>
            {
                _options.AccessTokenProvider = () => Task.FromResult(authKey);
            }
                )
            .WithAutomaticReconnect()
            .Build();

        connection.On<string>(nameof(UserDisconnectedAsync), UserDisconnectedAsync);        
        connection.On<string>(nameof(UserRejoinedAsync), UserRejoinedAsync);

        connection.On<List<string>>(nameof(ReceiveOldMessagesAsync), ReceiveOldMessagesAsync);
        
        connection.On<string>(nameof(ReceiveMessageAsync), ReceiveMessageAsync);   
        
        connection.On<string,string>(nameof(MatchFoundAsync), MatchFoundAsync);
        
        connection.On(nameof(MatchMakingStartedAsync),MatchMakingStartedAsync);

        try
        {
            await connection.StartAsync();
            _callBack?.Invoke(true);
        }
        catch (Exception _error)
        {
            Debug.Log($"Trying to connect with: {SERVER_URI} , authToken: {authKey}\nFailed with error: {_error}");
            _callBack?.Invoke(false);
        }
    }

    #region Receive messages

    private void UserDisconnectedAsync(string _userName)
    {
        Debug.Log("UserDisconnectedAsync: "+_userName);
        OnOpponentLeftRoom?.Invoke();
    }

    private void UserRejoinedAsync(string _userName)
    {
        Debug.Log("UserRejoinedAsync: "+_userName);
    }

    private void ReceiveOldMessagesAsync(List<string> _messages)
    {
        Debug.Log("ReceiveOldMessagesAsync: ");
        foreach (var _message in _messages)
        {
            Debug.Log(_message);
        }
    }
    
    private void ReceiveMessageAsync(string _message)
    {
        Debug.Log("ReceiveMessageAsync: "+_message);
        ExecuteMessage(JsonConvert.DeserializeObject<MessageData>(_message));
    }

    private void MatchFoundAsync(string _firstPlayer, string _secondPlayer)
    {
        Debug.Log($"MatchFoundAsync: {_firstPlayer} vs {_secondPlayer}");
        MatchData = new MatchData() { Players = new List<string>() { _firstPlayer, _secondPlayer } };
        OnOpponentJoinedRoom?.Invoke();
    }
    
    private void MatchMakingStartedAsync()
    {
        Debug.Log($"MatchMakingStartedAsync");
    }


    #endregion


    #region Send messages

    public new void SendMessage(string _message)
    {
        connection.SendAsync("SendMessageAsync", _message);
    }

    public void StartMatchMaking()
    {
        connection.SendAsync("MatchMakeAsync");
    }

    public void LeaveRoom()
    {
        //todo leave room
    }
    #endregion


    public void RegisterMessage(GameObject _object, string _functionName, string _data= null)
    {
        MessageData _message = new MessageData { GameObjectName = _object.name, MethodName = _functionName, Data = _data };
        string _messageJson = JsonConvert.SerializeObject(_message);
        Debug.Log("Sending message: "+ _messageJson);
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
        
        MonoBehaviour _targetComponent = _targetObject.GetComponent<MonoBehaviour>();
        if (_targetComponent == null)
        {
            Debug.LogError($"No MonoBehaviour found on object {_objectName}.");
        }

        string _methodName = _messageData.MethodName;
        Type _type = _targetComponent.GetType();
        MethodInfo _methodInfo = _type.GetMethod(_methodName);
                
        if (_methodInfo != null)
        {
            if (_messageData.Data == null)
            {
                _methodInfo.Invoke(_targetComponent, null);
            }
            else
            {
                _methodInfo.Invoke(_targetComponent, new object[] { _messageData.Data });
            }
        }
        else
        {
            Debug.LogError($"Method {_methodName} not found on object {_objectName}.");
        }
    }
}