using System;
using System.Collections;
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

    private void StartMatch()
    {
        switch (ModeHandler.Instance.Mode)
        {
            case GameMode.VsAi:
                ShowAIMatchMaking();
                break;
            case GameMode.VsPlayer:
                inputBlocker.SetActive(true);
                ShowPVPPanel();
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

    private void ShowPVPPanel()
    {
        if (!CanPlay)
        {
            return;
        }
        ManageInteractables(false);
    }

    private void JoinedRoom()
    {
        inputBlocker.SetActive(false);
        ManageInteractables(false);
        pvpPanel.Setup();
    }

    private void ILeftRoom()
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
