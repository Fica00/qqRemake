using TMPro;
using UnityEngine;

public class RankPointsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private void OnEnable()
    {
        PlayerData.UpdatedRankPoints += Show;

        Show();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedRankPoints -= Show;
    }

    private void Show()
    {
        display.text = DataManager.Instance.PlayerData.RankPoints.ToString();
    }
}