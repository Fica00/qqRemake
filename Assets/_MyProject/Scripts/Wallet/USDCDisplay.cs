using TMPro;
using UnityEngine;

public class USDCDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private bool showSuffix;

    private void OnEnable()
    {
        PlayerData.UpdatedUSDC += Show;
        Show();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedUSDC -= Show;
    }

    private void Show()
    {
        string _text = DataManager.Instance.PlayerData.USDT.ToString();
        if (showSuffix)
        {
            _text += " USDT";
        }
        display.text = _text;
    }
}
