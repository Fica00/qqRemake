using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MessageHelpers;


public class UIPVPPanel : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private MatchMakingPlayerDisplay myPlayer;
    [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
    [SerializeField] private GameObject matchingLabel;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private UIPlayPanel playPanel;
    private IEnumerator botRoutine;
    
    public void Setup()
    {
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MATCHMAKING);
        matchingLabel.SetActive(true);
        opponentPlayer.gameObject.SetActive(false);
        ManageInteractables(true);
        myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
        header.text = "Searching for opponent";
        gameObject.SetActive(true);
        TryShowTransition();

        // botRoutine = BringBot();
        // StartCoroutine(botRoutine);
    }

    private IEnumerator BringBot()
    {
        yield return new WaitForSeconds(7);
        SocketServerCommunication.OnILeftRoom += DoBringBot;
        SocketServerCommunication.Instance.LeaveRoom();
    }

    private void DoBringBot()
    {
        SocketServerCommunication.OnILeftRoom -= DoBringBot;
        Cancel();
        ModeHandler.Instance.Mode = GameMode.VsAi;
        playPanel.BringBot();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        ManageInteractables(true);

        cancelButton.onClick.AddListener(Cancel);
        SocketServerCommunication.OnOpponentJoinedRoom += OpponentJoined;
        SocketServerCommunication.OnRoomIsFull += RoomIsFull;
    }

    private void OnDisable()
    {
        cancelButton.onClick.RemoveListener(Cancel);

        SocketServerCommunication.OnOpponentJoinedRoom -= OpponentJoined;
        SocketServerCommunication.OnRoomIsFull += RoomIsFull;
        StopAllCoroutines();
    }

    private void RoomIsFull()
    {
        DialogsManager.Instance.OkDialog.Setup("Please select another room name");
        Close();
    }

    private void TryShowTransition()
    {
        if (SocketServerCommunication.Instance.MatchData==null)
        {
            Debug.Log(111);
            return;
        }

        if (SocketServerCommunication.Instance.MatchData.Players.Count!=2)
        {
            Debug.Log(222);
            return;
        }
        
        Debug.Log(333);
        LoadGameplay();
        RequestOpponentData();
    }

    private void Cancel()
    {
        ManageInteractables(false);
        SocketServerCommunication.OnILeftRoom += OnLeftRoom;
        if (ModeHandler.Instance.Mode == GameMode.VsPlayer)
        {
            SocketServerCommunication.Instance.CancelMatchMaking();
        }
        else
        {
            SocketServerCommunication.Instance.LeaveRoom();
        }
    }

    private void OnLeftRoom()
    {
        SocketServerCommunication.OnILeftRoom -= OnLeftRoom;
        playPanel.OnLeftRoom();
        Close();
    }

    private void Close()
    {
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);
        if (botRoutine!=default)
        {
            StopCoroutine(botRoutine);
            botRoutine = default;
        }
        gameObject.SetActive(false);
    }

    private void OpponentJoined()
    {
        ManageInteractables(false);
        RequestOpponentData();
        LoadGameplay();
    }

    private void LoadGameplay()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(4);
            UIMainMenu.Instance.ShowSceneTransition(() => { SceneManager.Instance.LoadPvpGameplay(false);});
        }
    }

    private void ManageInteractables(bool _status)
    {
        cancelButton.interactable = _status;
    }


    private void RequestOpponentData()
    {
        Debug.Log("Requesting opponent data");
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentAskedForMyData));
    }
    
    private void OpponentAskedForMyData()
    {
        Debug.Log("Sending my data to opponent");
        OpponentData _data = new OpponentData
        {
            Name = DataManager.Instance.PlayerData.Name, DeckName = DataManager.Instance.PlayerData.GetSelectedDeck().Name
        };
        
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(ShowOpponentData), JsonConvert.SerializeObject(_data));
    }

    private void ShowOpponentData(string _dataJson)
    {
        Debug.Log("Received data from opponent: "+_dataJson);
        OpponentData _data = JsonConvert.DeserializeObject<OpponentData>(_dataJson);
        opponentPlayer.Setup(
            _data.Name,
            _data.DeckName);
        opponentPlayer.gameObject.SetActive(true);
        header.text = "Opponent found!";
    }

}
