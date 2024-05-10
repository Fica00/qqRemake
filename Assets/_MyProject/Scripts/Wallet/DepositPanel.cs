using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DepositPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addressDisplay;
    [SerializeField] private Button copyAddress;
    public RawImage walletQR;

    private void OnEnable()
    {
        copyAddress.onClick.AddListener(CopyAddress);

        string _walletAddress = DataManager.Instance.PlayerData.UserWalletAddress;
        addressDisplay.text = _walletAddress;
        if (!string.IsNullOrEmpty(_walletAddress))
        {
            walletQR.texture = QrCreator.GenerateQr(_walletAddress);
            walletQR.color = Color.white;
        }
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
