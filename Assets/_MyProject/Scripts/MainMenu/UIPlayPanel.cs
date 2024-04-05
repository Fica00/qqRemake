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

    public static bool PlayAgain;

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

    private void Start()
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
        if (!PhotonManager.Instance.CanStartMatch)
        {
            PhotonManager.Instance.FixSelf();
            DialogsManager.Instance.OkDialog.Setup("Cleaning up the data, please try again in few moments");
            return;
        }
        
        switch (ModeHandler.Instance.Mode)
        {
            case GameMode.VsAi:
                ShowAIGameplay();
                break;
            case GameMode.VsPlayer:
                ShowPVPPanel();
                break;
            case GameMode.Friendly:
                DialogsManager.Instance.OkDialog.Setup("This feature is not implemented yet");
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

        UIMainMenu.Instance.ShowSceneTransition(() => { SceneManager.Instance.LoadAIGameplay(false);});
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
                DialogsManager.Instance.OkDialog.Setup("You need to have 12 qommons in deck");
                return false;
            }

            return true;
        }
    }
}
