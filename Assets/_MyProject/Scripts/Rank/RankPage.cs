using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPage : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private Image circle;
    [SerializeField] private TextMeshProUGUI rankNameDisplay;
    [SerializeField] private TextMeshProUGUI subRankDisplay;

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        Setup();
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void Setup()
    {
        RankData _rankData = RankSo.GetRankData(DataManager.Instance.PlayerData.Exp);
        circle.sprite = _rankData.RankSo.Sprite;
        rankNameDisplay.text = _rankData.RankSo.Name;
        subRankDisplay.text = _rankData.SubRank.ToString();
    }
}
