using UnityEngine;
using UnityEngine.UI;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] private Button playVSAIButton;
    [SerializeField] private Button playPVPButton;

    [Space()]
    [SerializeField]
    private UIPVPPanel pvpPanel;

    private void OnEnable()
    {
        playVSAIButton.onClick.AddListener(ShowAIGameplay);
        playPVPButton.onClick.AddListener(ShowPVPPanel);

        PhotonManager.OnIJoinedRoom += JoinedRoom;
        PhotonManager.OnILeftRoom += ILeftRoom;
    }

    private void OnDisable()
    {
        playVSAIButton.onClick.RemoveListener(ShowAIGameplay);
        playPVPButton.onClick.RemoveListener(ShowPVPPanel);

        PhotonManager.OnIJoinedRoom -= JoinedRoom;
        PhotonManager.OnILeftRoom -= ILeftRoom;
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
        PhotonManager.Instance.JoinRandomRoom();
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
        playVSAIButton.interactable = _status;
        playPVPButton.interactable = _status;
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
