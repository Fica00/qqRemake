using UnityEngine;
using UnityEngine.UI;

public class WalletPanel : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button receiveButton;
    [SerializeField] private Button sendButton;
    [SerializeField] private GameObject receivePanel;
    [SerializeField] private GameObject sendPanel;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        receiveButton.onClick.AddListener(ShowReceive);
        sendButton.onClick.AddListener(ShowSell);
        
        ManageInteractables(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        receiveButton.onClick.RemoveListener(ShowReceive);
        sendButton.onClick.RemoveListener(ShowSell);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void ShowReceive()
    {
        CloseAll();
        receivePanel.SetActive(true);
    }

    private void ShowSell()
    {
        CloseAll();
        sendPanel.SetActive(true);
    }

    private void CloseAll()
    {
        receivePanel.SetActive(false);
        sendPanel.SetActive(false);
    }

    public void ManageInteractables(bool _status)
    {
        closeButton.interactable = _status;
        receiveButton.interactable = _status;
        sendButton.interactable = _status;
    }
}
