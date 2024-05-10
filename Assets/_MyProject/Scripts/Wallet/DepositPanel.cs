using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DepositPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addressDisplay;
    [SerializeField] private Button copyAddress;

    private void OnEnable()
    {
        copyAddress.onClick.AddListener(CopyAddress);
        addressDisplay.text = DataManager.Instance.PlayerData.UserWalletAddress;
    }

    private void OnDisable()
    {
        copyAddress.onClick.RemoveListener(CopyAddress);
    }

    private void CopyAddress()
    {
        JavaScriptManager.Instance.CopyToClipboard(DataManager.Instance.PlayerData.UserWalletAddress);
    }
}
