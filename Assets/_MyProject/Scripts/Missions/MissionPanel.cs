using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [SerializeField] private MissionProgressDisplay missionProgressDisplay;
    [SerializeField] private Transform progressHolder;
    [SerializeField] private Button close;

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void Start()
    {
        ShowRewards();
    }

    private void ShowRewards()
    {
        foreach (var _reward in DataManager.Instance.GameData.MissionRewards.OrderBy(_mission => _mission.Count))
        {
            MissionProgressDisplay _rewardDisplay = Instantiate(missionProgressDisplay, progressHolder);
            bool _didUnlock = _reward.Count < DataManager.Instance.PlayerData.WeeklyMissionCount;
            _rewardDisplay.Setup(_didUnlock,_reward.Count);
        }
    }
}
