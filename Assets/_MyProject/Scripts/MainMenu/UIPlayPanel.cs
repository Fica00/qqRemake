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
        SceneManager.LoadAIGameplay();
    }

    private void ShowPVPPanel()
    {
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
}
