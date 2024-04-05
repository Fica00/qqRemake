using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections;
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

    private byte maxPlayersPerRoom = 2;

    public Room CurrentRoom => PhotonNetwork.CurrentRoom;

    public bool CanStartMatch => IsOnMasterServer && CanCreateRoom && PhotonNetwork.Server != ServerConnection.GameServer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = JavaScriptManager.Instance.IsDemo ? "demo" : "dev";
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

    public void FixSelf()
    {
        Reconnect();
        
        void Reconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                DoLeaveRoom();
            }
            else
            {
                OnFinishedInit += FinishedConnecting;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        
        void FinishedConnecting()
        {
            OnFinishedInit -= FinishedConnecting;
            DoLeaveRoom();
        }

        void  DoLeaveRoom()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.NetworkClientState != ClientState.Leaving)
            {
                OnILeftRoom += FinishDoSomething;
                DoLeaveRoom();
            }
            else
            {
                FinishFixSelf();
            }
        }

        void FinishDoSomething()
        {
            OnILeftRoom -= FinishDoSomething;
            FinishFixSelf();
        }

        void FinishFixSelf()
        {
            StartCoroutine(DelayCall());
            IEnumerator DelayCall()
            {
                yield return new WaitForSeconds(1);
            }
        }
        
    }


    public override void OnConnectedToMaster()
    {
        isInit = true;
        OnFinishedInit?.Invoke();
    }

    public void JoinRandomRoom()
    {
        SetPhotonPlayerProperties();
        TryJoinRandomRoom();
    }

    private void SetPhotonPlayerProperties()
    {
        Hashtable _myProperties = new Hashtable
            {
                [NAME] = DataManager.Instance.PlayerData.Name,
                [DECK_NAME] = DataManager.Instance.PlayerData.GetSelectedDeck().Name,
                [AMOUNT_OF_CARDS_IN_COLLECTION] = DataManager.Instance.PlayerData.OwnedQoomons.Count,
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

    private void TryJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short _returnCode, string _message)
    {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short _returnCode, string _message)
    {
        CreateRoom();
    }

    private void CreateRoom()
    {
        StartCoroutine(TryCreateRoom());
        
        IEnumerator TryCreateRoom()
        {
            yield return new WaitForSeconds(.3f);
            RoomOptions _roomOptions = new RoomOptions { IsOpen = true, MaxPlayers = maxPlayersPerRoom };
            PhotonNetwork.CreateRoom(null, _roomOptions, TypedLobby.Default);
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