using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPVPPanel : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private MatchMakingPlayerDisplay myPlayer;
    [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
    [SerializeField] private GameObject matchingLabel;

    public void Setup()
    {
        matchingLabel.SetActive(true);
        opponentPlayer.gameObject.SetActive(false);
        ManageInteractables(true);
        myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        ManageInteractables(true);

        cancelButton.onClick.AddListener(Cancel);
        PhotonManager.OnIJoinedRoom += TryShowTransition;
        PhotonManager.OnILeftRoom += Close;
        PhotonManager.OnOpponentJoinedRoom += OpponentJoined;
    }

    private void OnDisable()
    {
        cancelButton.onClick.RemoveListener(Cancel);

        PhotonManager.OnIJoinedRoom -= TryShowTransition;
        PhotonManager.OnILeftRoom -= Close;
        PhotonManager.OnOpponentJoinedRoom -= OpponentJoined;
    }

    private void TryShowTransition()
    {
        if (PhotonManager.Instance.CurrentRoom.PlayerCount==2)
        {
            LoadGameplay();
            ShowOpponent();
        }
    }

    private void Cancel()
    {
        ManageInteractables(false);
        PhotonManager.Instance.LeaveRoom();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void OpponentJoined()
    {
        ManageInteractables(false);
        ShowOpponent();
        LoadGameplay();
    }

    private void ShowOpponent()
    {
        opponentPlayer.Setup(
            PhotonManager.Instance.GetOpponentsProperty(PhotonManager.NAME),
            PhotonManager.Instance.GetOpponentsProperty(PhotonManager.DECK_NAME));
        opponentPlayer.gameObject.SetActive(true);
    }

    private void LoadGameplay()
    {
        if (PhotonManager.Instance.IsMasterClient)
        {
            UIMainMenu.Instance.ShowSceneTransition(SceneManager.LoadPVPGameplay);
        }
        else
        {
            UIMainMenu.Instance.ShowSceneTransition(null);
        }
    }

    private void ManageInteractables(bool _status)
    {
        cancelButton.interactable = _status;
    }
    
}
