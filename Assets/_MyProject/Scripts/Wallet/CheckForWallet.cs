using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckForWallet : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private RequestWallet requestWallet;
    [SerializeField] private Button close;
    [SerializeField] private WalletPanel walletPanel;

    private void OnEnable()
    {
        PlayerData.UpdatedUserWalletAddress += CheckStatus;
        close.onClick.AddListener(Close);
        
        holder.SetActive(true);
        statusText.text = "Checking for wallet...";
        CheckStatus();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedUserWalletAddress -= CheckStatus;
        close.onClick.RemoveListener(Close);
    }

    public void CheckStatus()
    {
        if (!DataManager.Instance.PlayerData.DidRequestUserWallet)
        {
            requestWallet.Setup();
        }
        else if (string.IsNullOrEmpty(DataManager.Instance.PlayerData.UserWalletAddress))
        {
            statusText.text = "Waiting for server to assign wallet address";
        }
        else
        {
            holder.SetActive(false);
        }
    }

    private void Close()
    {
        holder.SetActive(false);
        walletPanel.Close();
    }
}
