using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankDisplay : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private TextMeshProUGUI rankName;

    private void OnEnable()
    {
        PlayerData.UpdatedExp += ShowCurrentRank;
        ShowCurrentRank();
        ShowRewards();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedExp -= ShowCurrentRank;
    }

    private void ShowCurrentRank()
    {
        RankData _rank = RankSo.GetRankData(DataManager.Instance.PlayerData.RankPoints);
        circle.sprite = _rank.RankSo.Sprite;
        progress.fillAmount = _rank.Percentage;
        progressDisplay.text = $"{_rank.PointsOnRank}/{_rank.RankSo.RequirementPerSubRank}";
        rankName.text = $"{_rank.RankSo.Name} ({_rank.SubRank})";
    }

    private void ShowRewards()
    {

    }
}