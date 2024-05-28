using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SocketServerCommunication : MonoBehaviour
{
    public static SocketServerCommunication Instance;
    private const string SERVER_URI = "http://localhost:8080/hubs/communication";
    private HubConnection connection;
    private string authKey;

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

    public async void Init()
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

        await connection.StartAsync();
    }

    #region Receive messages

    private void UserDisconnectedAsync(string _userName)
    {
        Debug.Log("UserDisconnectedAsync: "+_userName);
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
    }

    private void MatchFoundAsync(string _firstPlayer, string _secondPlayer)
    {
        Debug.Log($"MatchFoundAsync: {_firstPlayer} vs {_secondPlayer}");
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
    #endregion
    
}