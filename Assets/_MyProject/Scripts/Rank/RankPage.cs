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
        RankData _rankData = RankSo.GetRankData(DataManager.Instance.PlayerData.RankPoints);
        circle.sprite = _rankData.RankSo.Sprite;
        rankNameDisplay.text = _rankData.RankSo.Name;
        subRankDisplay.text = _rankData.Level.ToString();
        
        //ShowRewards();
        label.SetActive(DataManager.Instance.PlayerData.AmountOfRankGamesPlayed < 10);
    }
    
    private void TryClaim(RankReward _reward)
    {
        if (RankSo.GetRankLevel(DataManager.Instance.PlayerData.RankPoints)<_reward.RankLevel)
        {
            return;
        }

        if (DataManager.Instance.PlayerData.ClaimedRankRewards.Contains(_reward.RankIndex))
        {
            return;
        }

        if (DataManager.Instance.PlayerData.AmountOfRankGamesPlayed < 10)
        {
            return;   
        }
        
        DataManager.Instance.PlayerData.ClaimReward(_reward.ItemType, _reward.Value);
        DataManager.Instance.PlayerData.ClaimRankReward(_reward.RankIndex);
        SceneManager.Instance.ReloadScene();
    }

    private void ShowRewards()
    {
        var _rewards = DataManager.Instance.GameData.RankRewards.OrderBy(_reward => _reward.RankLevel).ToList();
        int _counter = 0;
        int _playerLevel = RankSo.GetRankLevel(DataManager.Instance.PlayerData.RankPoints);
                    Debug.Log(_playerLevel);
        for (int _i = 0; _i < _rewards.Count(); _i++)
        {
            var _rankReward = _rewards[_i];
            if (_i!=0)
            {
                int _rankLevel = _rewards[_i-1].RankLevel;
                for (int _index = 0; _index < _rankReward.RankLevel-_rewards[_i-1].RankLevel; _index++)
                {
                    Debug.Log(_rankLevel);
                    _counter++;
                    GameObject _bar = Instantiate(barPrefab, rewardHolder);
                    _bar.GetComponentInChildren<Image>().sprite = _rankLevel<= _playerLevel
                        ? barFull
                        : barEmpty;
                    _rankLevel++;
                }
            }
            GameRankRewardDisplay _display = Instantiate(rewardPrefab, rewardHolder);
            _display.Setup(_rankReward);
        }
    }
}
