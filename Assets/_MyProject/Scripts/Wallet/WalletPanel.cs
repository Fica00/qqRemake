using System;
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
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        receiveButton.onClick.RemoveListener(ShowReceive);
        sendButton.onClick.RemoveListener(ShowSell);
    }

    private void Close()
    {
        Destroy(gameObject);
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
}
