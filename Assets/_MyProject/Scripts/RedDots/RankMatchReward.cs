using UnityEngine;
using UnityEngine.UI;

namespace RedDot
{
    public class RankMatchReward : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            Check();
        }

        private void Check()
        {
            foreach (var _rankReward in DataManager.Instance.GameData.RankRewards)
            {
                if (_rankReward.AmountOfMatches>DataManager.Instance.PlayerData.AmountOfRankGamesPlayed)
                {
                    continue;
                }

                if (DataManager.Instance.PlayerData.ClaimedRankRewards.Contains(_rankReward.AmountOfMatches))
                {
                    continue;
                }

                image.enabled = true;
                return;
            }

            image.enabled = false;
        }
    }
}