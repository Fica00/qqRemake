using UnityEngine;
using UnityEngine.UI;

namespace RedDot
{
    public class LevelReward : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            PlayerData.UpdatedExp += Check;
            Check();
        }

        private void OnDisable()
        {
            PlayerData.UpdatedExp -= Check;
        }

        private void Check()
        {
            foreach (var _levelReward in DataManager.Instance.GameData.LevelRewards)
            {
                if (_levelReward.Level>DataManager.Instance.PlayerData.Level)
                {
                    continue;
                }

                if (DataManager.Instance.PlayerData.ClaimedLevelRewards.Contains(_levelReward.Level))
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

