using UnityEngine;
using UnityEngine.UI;

namespace RedDot
{
    public class MissionReward : MonoBehaviour
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
            image.enabled = false;
            CheckLogin();
            CheckMissions();
        }

        private void CheckLogin()
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
                break;
            }
        }

        private void CheckMissions()
        {
            foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
            {
                if (_missionProgress.Claimed)
                {
                    continue;
                }

                if (!_missionProgress.Completed)
                {
                    continue;
                }
                image.enabled = true;
                break;
            }
            
        }
    }
}