using TMPro;
using UnityEngine;

public class DepositPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addressDisplay;

    private void OnEnable()
    {

        addressDisplay.text = DataManager.Instance.PlayerData.UserWalletAddress;
    }

    private void OnDisable()
    {
        
    }
}
