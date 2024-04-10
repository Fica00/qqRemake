using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPVPPanel : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private MatchMakingPlayerDisplay myPlayer;
    [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
    [SerializeField] private GameObject matchingLabel;
    [SerializeField] private TextMeshProUGUI header;


    public void Setup()
    {
        matchingLabel.SetActive(true);
        opponentPlayer.gameObject.SetActive(false);
        ManageInteractables(true);
        myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
        header.text = "Searching for opponent";
        gameObject.SetActive(true);
        TryShowTransition();
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
        header.text = "Opponent found!";
    }

    private void LoadGameplay()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2);
            if (PhotonManager.Instance.IsMasterClient)
            {
                UIMainMenu.Instance.ShowSceneTransition(() => { SceneManager.Instance.LoadPvpGameplay(false);});
                PhotonManager.Instance.CloseRoom();
            }
            else
            {
                UIMainMenu.Instance.ShowSceneTransition(null);
            }
        }
    }

    private void ManageInteractables(bool _status)
    {
        cancelButton.interactable = _status;
    }
    
}
