using UnityEngine;
using UnityEngine.UI;

namespace RedDot
{
    public class LoginReward : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            PlayerData.UpdatedWeeklyLoginAmount += Check;
            Check();
        }
        
        private void OnDisable()
        {
            PlayerData.UpdatedWeeklyLoginAmount -= Check;
        }

        private void Check()
        {
            foreach (var _loginReward in DataManager.Instance.GameData.LoginRewards)
            {
                if (_loginReward.Days>DataManager.Instance.PlayerData.WeeklyLoginAmount)
                {
                    continue;
                }

                if (DataManager.Instance.PlayerData.ClaimedLoginRewards.Contains(_loginReward.Days))
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