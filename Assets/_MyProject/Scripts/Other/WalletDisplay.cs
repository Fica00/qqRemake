using TMPro;
using UnityEngine;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private TextMeshProUGUI usdcDisplay;

    private void OnEnable()
    {
        PlayerData.UpdatedCoins += ShowCoins;
        PlayerData.UpdatedUSDC += ShowUSDC;
        ShowCoins();
        ShowUSDC();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedCoins -= ShowCoins;
        PlayerData.UpdatedUSDC -= ShowUSDC;
    }

    private void ShowCoins()
    {
        coinsDisplay.text = DataManager.Instance.PlayerData.Coins.ToString();
    }

    private void ShowUSDC()
    {
        usdcDisplay.text = DataManager.Instance.PlayerData.USDC.ToString();
    }
}
