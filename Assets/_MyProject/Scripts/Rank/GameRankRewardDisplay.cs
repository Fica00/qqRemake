using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRankRewardDisplay : MonoBehaviour
{
    public static Action<RankReward> OnClicked;
    
    [SerializeField] private Image rewardImage;
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject unlockedDisplay;
    [SerializeField] private GameObject claimedDisplay;
    [SerializeField] private TextMeshProUGUI rewardName;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI numberOfMatches;
    
    private RankReward reward;

    private void OnEnable()
    {
        button.onClick.AddListener(Press);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Press);
    }

    private void Press()
    {
        OnClicked?.Invoke(reward);
    }

    public void Setup(RankReward _reward)
    {
        reward = _reward;
        numberOfMatches.text = reward.AmountOfMatches.ToString();

        if (_reward.AmountOfMatches>DataManager.Instance.PlayerData.AmountOfRankGamesPlayed)
        {
            cloud.SetActive(true);
        }
        else
        {
            rewardImage.sprite = _reward.ItemType==ItemType.Qoomon 
                ? SpriteProvider.Instance.GetQoomonSprite(_reward.Value) 
                : SpriteProvider.Instance.Get(_reward.ItemType);
            rewardName.text = _reward.Name;
            if (DataManager.Instance.PlayerData.ClaimedRankRewards.Contains(_reward.AmountOfMatches))
            {
                claimedDisplay.SetActive(true);
            }
            else
            {
                unlockedDisplay.SetActive(true);
            }
        }
    }
}
