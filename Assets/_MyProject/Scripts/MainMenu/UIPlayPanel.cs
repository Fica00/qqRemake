using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button overlayPlayButton;
    [SerializeField] private GameObject inputBlocker;
    [Space()]
    [SerializeField] private UIPVPPanel pvpPanel;
    [SerializeField] private UIMatchMakingVsBot matchMakingVsBot;
    [SerializeField] private UIFriendlyPanel friendlyPanel;

    public static bool PlayAgain;
    
    private void OnEnable()
    {
        playButton.onClick.AddListener(StartMatch);
        overlayPlayButton.onClick.AddListener(StartMatch);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartMatch);
        overlayPlayButton.onClick.RemoveListener(StartMatch);
    }
    
    public void TryAutoMatch()
    {
        if (!PlayAgain)
        {
            return;
        }
        PlayAgain = false;
        StartMatch();
    }

    private void StartFriendlyMatch()
    {
        ModeHandler.Instance.Mode = GameMode.Friendly;
        StartMatch();
    }

    private void StartMatch()
    {
        switch (ModeHandler.Instance.Mode)
        {
            case GameMode.VsAi:
                ShowAIMatchMaking();
                break;
            case GameMode.VsPlayer:
                ConnectWithServer(ShowPvp);
                break;
            case GameMode.Friendly:
                ConnectWithServer(ShowFriendly);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowAIMatchMaking()
    {
        if (!CanPlay)
        {
            return;
        }

        matchMakingVsBot.Setup(Random.Range(3f,7f));
    }

    private void ConnectWithServer(Action _callBack)
    {
        if (!CanPlay)
        {
            return;
        }
        
        ManageInteractables(false);
        inputBlocker.SetActive(true);
        
        SocketServerCommunication.OnInitFinished += HandleConnection;
        SocketServerCommunication.Instance.Init();
        
        void HandleConnection(bool _status)
        {
            SocketServerCommunication.OnInitFinished -= HandleConnection;
            inputBlocker.SetActive(false);
            ManageInteractables(true);
            if (!_status)
            {
                DialogsManager.Instance.OkDialog.Setup("Something went wrong while connecting with server, please try again later");
                return;
            }
            
            _callBack?.Invoke();
        }
    }

    private void ShowPvp()
    {
        pvpPanel.Setup();
        SocketServerCommunication.Instance.StartMatchMaking();
    }

    private void ShowFriendly()
    {
        friendlyPanel.Setup();
    }

    public void OnLeftRoom()
    {
        ManageInteractables(true);
    }

    private void ManageInteractables(bool _status)
    {
        playButton.interactable = _status;
    }

    private bool CanPlay
    {
        get
        {
            if (DataManager.Instance.PlayerData.CardIdsInDeck.Count!=12)
            {
                DialogsManager.Instance.OkDialog.Setup("You need to have 12 qommons in deck");
                return false;
            }

            return true;
        }
    }

    public void BringBot()
    {
        matchMakingVsBot.Setup(0);
    }
}
