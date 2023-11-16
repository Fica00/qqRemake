using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    public static Action OnFinishedInit;
    public static Action OnIJoinedRoom;
    public static Action OnOpponentJoinedRoom;
    public static Action OnILeftRoom;
    public static Action OnOpponentLeftRoom;
    private static bool isInit;
    private List<string> roomNames = new() { "Room1","Room2","Room3","Room4","Room5","Room6","Room7","Room8","Room9","Room10","Room11","Room12","Room13","Room14","Room15"};

    private byte maxPlayersPerRoom = 2;
    private int roomNameIndex = 0;
    private int roomTriesCounter = 0;

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
        Hashtable myProperties = new Hashtable();
        myProperties["name"] = UnityEngine.Random.Range(0, 100);
        PhotonNetwork.LocalPlayer.CustomProperties = myProperties;
    }

    private void TryJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNames[roomNameIndex]);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
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