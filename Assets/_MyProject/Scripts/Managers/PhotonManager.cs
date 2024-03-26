using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public const string NAME = "name";
    public const string DECK_NAME = "deckName";
    public const string AMOUNT_OF_CARDS_IN_HAND = "amountOfCardsInHand";
    public const string AMOUNT_OF_DISCARDED_CARDS = "amountOfDiscardedCards";
    public const string AMOUNT_OF_DESTROYED_CARDS = "amountOfDestroyedCards";
    public const string AMOUNT_OF_CARDS_IN_COLLECTION = "amountOfCardsInCollection";
    public static PhotonManager Instance;
    public static Action OnFinishedInit;
    public static Action OnIJoinedRoom;
    public static Action OnOpponentJoinedRoom;
    public static Action OnILeftRoom;
    public static Action OnOpponentLeftRoom;
    private static bool isInit;
    public static bool IsOnMasterServer=> PhotonNetwork.Server== ServerConnection.MasterServer;
    public static bool CanCreateRoom=> PhotonNetwork.NetworkClientState== ClientState.ConnectedToMasterServer;

    public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    private List<string> roomNames = new() { "Room1","Room2","Room3","Room4","Room5","Room6","Room7","Room8","Room9","Room10","Room11","Room12","Room13","Room14","Room15"};

    private byte maxPlayersPerRoom = 2;
    private int roomNameIndex = 0;
    private int roomTriesCounter = 0;

    public Room CurrentRoom => PhotonNetwork.CurrentRoom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (isInit)
        {
            OnFinishedInit?.Invoke();
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        isInit = true;
        OnFinishedInit?.Invoke();
    }

    public void JoinRandomRoom()
    {
        roomNameIndex = 0;
        roomTriesCounter = 0;
        SetPhotonPlayerProperties();
        TryJoinRoom();
    }

    private void SetPhotonPlayerProperties()
    {
        Hashtable _myProperties = new Hashtable
            {
                [NAME] = DataManager.Instance.PlayerData.Name,
                [DECK_NAME] = DataManager.Instance.PlayerData.GetSelectedDeck().Name,
                [AMOUNT_OF_CARDS_IN_COLLECTION] = DataManager.Instance.PlayerData.OwnedQommons.Count,
                [AMOUNT_OF_CARDS_IN_HAND] = 0,
                [AMOUNT_OF_DESTROYED_CARDS] = 0,
                [AMOUNT_OF_DISCARDED_CARDS] = 0
            };
        PhotonNetwork.LocalPlayer.CustomProperties = _myProperties;
    }

    public void TryUpdateCustomProperty(string _key, string _value)
    {
        if (PhotonNetwork.CurrentRoom is null or null)
        {
            return;
        }
        Hashtable _existingProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!_existingProperties.ContainsKey(_key))
        {
            return;
        }
        _existingProperties[_key] = _value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_existingProperties);
    }

    public string GetOpponentsProperty(string _key)
    {
        Player _opponent = default;
        foreach (var _potentialOpponent in PhotonNetwork.CurrentRoom.Players)
        {
            if (_potentialOpponent.Value.IsLocal)
            {
                continue;
            }

            _opponent = _potentialOpponent.Value;
            break;
        }

        return _opponent.CustomProperties[_key].ToString();
    }

    private void TryJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNames[roomNameIndex]);
    }
    
    public override void OnJoinRoomFailed(short _returnCode, string _message)
    {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short _returnCode, string _message)
    {
        TryJoinRoom();
    }

    private void CreateRoom()
    {
        StartCoroutine(TryCreateRoom());
        
        IEnumerator TryCreateRoom()
        {
            yield return new WaitForSeconds(.3f);
            RoomOptions _roomOptions = new RoomOptions { IsOpen = true, MaxPlayers = maxPlayersPerRoom };
            roomTriesCounter++;
            if (roomTriesCounter%3==0)
            {
                roomNameIndex++;
            }
            if (roomNameIndex>= roomNames.Count)
            {
                roomNameIndex = 0;
            }
            PhotonNetwork.CreateRoom(roomNames[roomNameIndex], _roomOptions, TypedLobby.Default);
        }
    }
    

    public override void OnJoinedRoom()
    {
        OnIJoinedRoom?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player _newPlayer)
    {
        OnOpponentJoinedRoom?.Invoke();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        OnILeftRoom?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player _otherPlayer)
    {
        OnOpponentLeftRoom?.Invoke();
    }
}