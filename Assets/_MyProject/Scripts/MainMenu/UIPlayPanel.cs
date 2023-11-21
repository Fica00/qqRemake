using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;

    [Space()]
    [SerializeField]
    private UIPVPPanel pvpPanel;

    private void OnEnable()
    {
        playButton.onClick.AddListener(StartMatch);

        PhotonManager.OnIJoinedRoom += JoinedRoom;
        PhotonManager.OnILeftRoom += ILeftRoom;
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartMatch);

        PhotonManager.OnIJoinedRoom -= JoinedRoom;
        PhotonManager.OnILeftRoom -= ILeftRoom;
    }

    private void StartMatch()
    {
        switch (ModeHandler.Instance.Mode)
        {
            case GameMode.VsAi:
                ShowAIGameplay();
                break;
            case GameMode.VsPlayer:
                ShowPVPPanel();
                break;
            case GameMode.Friendly:
                UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowAIGameplay()
    {
        if (!CanPlay)
        {
            return;
        }
        SceneManager.LoadAIGameplay();
    }

    private void ShowPVPPanel()
    {
        if (!CanPlay)
        {
            return;
        }
        ManageInteractables(false);
        StartCoroutine(SearchForMatch());
        IEnumerator SearchForMatch()
        {
            while (!PhotonManager.IsOnMasterServer || !PhotonManager.CanCreateRoom)
            {
                yield return null;
            }
            
            PhotonManager.Instance.JoinRandomRoom();
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void JoinedRoom()
    {
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
                UIManager.Instance.OkDialog.Setup("You need to have 12 qommons in deck");
                return false;
            }

            return true;
        }
    }
}
