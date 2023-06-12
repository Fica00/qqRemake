using UnityEngine;
using UnityEngine.UI;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] Button playVSAIButton;
    [SerializeField] Button playPVPButton;

    [Space()]
    [SerializeField] UIPVPPanel pvpPanel;

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

    void ShowAIGameplay()
    {
        SceneManager.LoadAIGameplay();
    }

    void ShowPVPPanel()
    {
        ManageInteractables(false);
        PhotonManager.Instance.JoinRandomRoom();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void JoinedRoom()
    {
        ManageInteractables(false);
        pvpPanel.Setup();
    }

    void ILeftRoom()
    {
        ManageInteractables(true);
    }

    void ManageInteractables(bool _status)
    {
        playVSAIButton.interactable = _status;
        playPVPButton.interactable = _status;
    }
}
