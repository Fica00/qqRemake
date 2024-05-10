using UnityEngine;
using UnityEngine.UI;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private Button walletButton;
    [SerializeField] private GameObject wallet;

    private void OnEnable()
    {
        walletButton.onClick.AddListener(ShowWallet);
    }

    private void OnDisable()
    {
        walletButton.onClick.RemoveListener(ShowWallet);
    }

    private void ShowWallet()
    {
        wallet.SetActive(true);
    }
}
