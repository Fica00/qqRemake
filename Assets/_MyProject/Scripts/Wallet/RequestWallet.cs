using UnityEngine;
using UnityEngine.UI;

public class RequestWallet : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Button requestWallet;
    [SerializeField] private Button close;
    [SerializeField] private WalletPanel walletPanel;
    [SerializeField] private CheckForWallet checkForWallet;
    
    public void Setup()
    {
        holder.SetActive(true);    
    }

    private void OnEnable()
    {
        requestWallet.onClick.AddListener(Request);
        close.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        requestWallet.onClick.RemoveListener(Request);
        close.onClick.RemoveListener(Close);
    }

    private void Request()
    {
        DataManager.Instance.PlayerData.DidRequestUserWallet = true;
        holder.SetActive(false);
        checkForWallet.CheckStatus();
    }

    private void Close()
    {
        holder.SetActive(false);
        walletPanel.Close();
    }
}
