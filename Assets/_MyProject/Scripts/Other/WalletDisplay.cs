using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private TextMeshProUGUI usdcDisplay;
    [SerializeField] private Button walletButton;
    [SerializeField] private GameObject walletPrefab;

    private void OnEnable()
    {
        PlayerData.UpdatedCoins += ShowCoins;
        PlayerData.UpdatedUSDC += ShowUSDC;
        ShowCoins();
        ShowUSDC();
        walletButton.onClick.AddListener(ShowWallet);
    }

    private void OnDisable()
    {
        PlayerData.UpdatedCoins -= ShowCoins;
        PlayerData.UpdatedUSDC -= ShowUSDC;
        walletButton.onClick.RemoveListener(ShowWallet);
    }

    private void ShowCoins()
    {
        coinsDisplay.text = DataManager.Instance.PlayerData.Coins.ToString();
    }

    private void ShowUSDC()
    {
        usdcDisplay.text = DataManager.Instance.PlayerData.USDC.ToString();
    }

    private void ShowWallet()
    {
        Instantiate(walletPrefab);
    }
}
