using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPage : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private Image circle;
    [SerializeField] private TextMeshProUGUI rankNameDisplay;
    [SerializeField] private TextMeshProUGUI subRankDisplay;
    [SerializeField] private GameObject label;
    
    [SerializeField] private GameRankRewardDisplay rewardPrefab;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform rewardHolder;

    [SerializeField] private Sprite barEmpty;
    [SerializeField] private Sprite barFull;

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        GameRankRewardDisplay.OnClicked += TryClaim;
        
        Setup();
    }

    private void OnDisable()
    {
        GameRankRewardDisplay.OnClicked -= TryClaim;
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
        
        ShowRewards();
        label.SetActive(DataManager.Instance.PlayerData.AmountOfRankGamesPlayed < 10);
    }
    
    private void TryClaim(RankReward _reward)
    {
        if (DataManager.Instance.PlayerData.AmountOfRankGamesPlayed<_reward.AmountOfMatches)
        {
            return;
        }

        if (DataManager.Instance.PlayerData.ClaimedRankRewards.Contains(_reward.AmountOfMatches))
        {
            return;
        }
        
        DataManager.Instance.PlayerData.ClaimReward(_reward.ItemType, _reward.Value);
        DataManager.Instance.PlayerData.ClaimRankReward(_reward.AmountOfMatches);
        SceneManager.Instance.ReloadScene();
    }

    private void ShowRewards()
    {
        var _rewards = DataManager.Instance.GameData.RankRewards.OrderBy(_reward => _reward.AmountOfMatches).ToList();
        int _counter = 0;
        for (int _i = 0; _i < _rewards.Count(); _i++)
        {
            var _rankReward = _rewards[_i];
            if (_i!=0)
            {
                for (int _index = 0; _index < _rankReward.AmountOfMatches-_rewards[_i-1].AmountOfMatches; _index++)
                {
                    _counter++;
                    GameObject _bar = Instantiate(barPrefab, rewardHolder);
                    _bar.GetComponentInChildren<Image>().sprite = _counter< DataManager.Instance.PlayerData.AmountOfRankGamesPlayed
                        ? barFull
                        : barEmpty;
                }
            }
            GameRankRewardDisplay _display = Instantiate(rewardPrefab, rewardHolder);
            _display.Setup(_rankReward);
        }
    }
}
