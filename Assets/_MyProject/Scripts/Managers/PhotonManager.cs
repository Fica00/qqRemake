using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    public static Action OnFinishedInit;
    public static Action OnIJoinedRoom;
    public static Action OnOpponentJoinedRoom;
    public static Action OnILeftRoom;
    public static Action OnOpponentLeftRoom;
    static bool isInit;

    byte maxPlayersPerRoom = 2;

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
        SetPhotonPlayerProperties();
        PhotonNetwork.JoinRandomRoom(null, maxPlayersPerRoom);
    }

    void SetPhotonPlayerProperties()
    {
        Hashtable myProperties = new Hashtable();
        myProperties["name"] = UnityEngine.Random.Range(0, 100);
        PhotonNetwork.LocalPlayer.CustomProperties = myProperties;
    }

    public override void OnJoinRandomFailed(short _returnCode, string _message)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = maxPlayersPerRoom;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
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