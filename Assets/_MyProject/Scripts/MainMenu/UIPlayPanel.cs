using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject inputBlocker;
    [Space()]
    [SerializeField] private UIPVPPanel pvpPanel;
    [SerializeField] private UIMatchMakingVsBot matchMakingVsBot;

    private void OnEnable()
    {
        playButton.onClick.AddListener(StartMatch);

        SocketServerCommunication.OnILeftRoom += OnLeftRoom;
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartMatch);
        
        SocketServerCommunication.OnILeftRoom -= OnLeftRoom;
    }

    private void StartMatch()
    {
        switch (ModeHandler.Instance.Mode)
        {
            case GameMode.VsAi:
                ShowAIMatchMaking();
                break;
            case GameMode.VsPlayer:
                ConnectWithServer();
                break;
            case GameMode.Friendly:
                DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
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

    private void ConnectWithServer()
    {
        if (!CanPlay)
        {
            return;
        }
        
        ManageInteractables(false);
        inputBlocker.SetActive(true);
        SocketServerCommunication.Instance.Init(ShowPvp);
    }

    private void ShowPvp(bool _status)
    {
        inputBlocker.SetActive(false);
        if (!_status)
        {
            ManageInteractables(true);
            DialogsManager.Instance.OkDialog.Setup("Something went wrong while connecting with server, please try again later");
            return;
        }
        
        SocketServerCommunication.Instance.StartMatchMaking();
        pvpPanel.Setup();
    }

    private void OnLeftRoom()
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
