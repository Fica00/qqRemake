using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private GameObject levelHolder;
    [SerializeField] private Image progressFill;

    private void OnEnable()
    {
        PlayerData.UpdatedExp += Show;
        
        Show();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedExp -= Show;
    }

    private void Show()
    {
        level.text = DataManager.Instance.PlayerData.Level.ToString();
        progressDisplay.text = $"{DataManager.Instance.PlayerData.CurrentExpOnLevel}/{DataManager.Instance.PlayerData.GetXpForNextLevel()}";
        progressFill.fillAmount = DataManager.Instance.PlayerData.LevelPercentage;
        levelHolder.SetActive(!DataManager.Instance.PlayerData.IsMaxLevel);
    }
}
