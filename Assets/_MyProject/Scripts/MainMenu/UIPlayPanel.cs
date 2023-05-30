using UnityEngine;
using UnityEngine.UI;

public class UIPlayPanel : MonoBehaviour
{
    [SerializeField] Button playVSAIButton;
    [SerializeField] Button playPVPButton;
    [SerializeField] Button closeButton;

    [Space()]
    [SerializeField] UIPVPPanel pvpPanel;

    public void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        playVSAIButton.onClick.AddListener(ShowAIGameplay);
        playPVPButton.onClick.AddListener(ShowPVPPanel);
        closeButton.onClick.AddListener(Close);

        PhotonManager.OnIJoinedRoom += JoinedRoom;
        PhotonManager.OnILeftRoom += ILeftRoom;
    }

    private void OnDisable()
    {
        playVSAIButton.onClick.RemoveListener(ShowAIGameplay);
        playPVPButton.onClick.RemoveListener(ShowPVPPanel);
        closeButton.onClick.RemoveListener(Close);

        PhotonManager.OnIJoinedRoom -= JoinedRoom;
        PhotonManager.OnILeftRoom -= ILeftRoom;
    }

    void ShowAIGameplay()
    {
        SceneManager.LoadAIGameplay();
    }

    void ShowPVPPanel()
    {
        UIManager.Instance.OkDialog.Setup("PVP is not ready yet");
        return;
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
        closeButton.interactable = _status;
    }
}
