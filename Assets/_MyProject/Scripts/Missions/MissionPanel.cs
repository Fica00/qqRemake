using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [SerializeField] private LoginProgressDisplay loginProgressDisplay;
    [SerializeField] private Transform progressHolder;
    [SerializeField] private Button close;
    [SerializeField] private QoomonUnlockingPanel qoomonUnlockingPanel;
    [SerializeField] private TextMeshProUGUI loggedInText;
    [SerializeField] private TextMeshProUGUI numberOfTasksCompleted;
    [SerializeField] private MissionDisplay missionDisplay;
    [SerializeField] private Transform missionHolder;

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        LoginProgressDisplay.OnClicked += TryClaim;
        MissionManager.OnClaimed += ShowCompletedText;
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
        LoginProgressDisplay.OnClicked -= TryClaim;
        MissionManager.OnClaimed += ShowCompletedText;
    }

    private void ShowCompletedText(MissionProgress _obj)
    {
        ShowCompletedText();
    }

    private void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void Start()
    {
        ShowLoginRewards();
        loggedInText.text = $"{DataManager.Instance.PlayerData.WeeklyLoginAmount}/7 days";
        ShowCompletedText();
        ShowMissions();
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        while (gameObject.activeSelf)
        {
            ShowCompletedText();
            yield return new WaitForSeconds(1);
        }
    }

    private void ShowCompletedText()
    {
        numberOfTasksCompleted.text = $"<size=62>Daily missions</size>\nwill be refreshed in {GetRefreshTime()}\n{DataManager.Instance.PlayerData.MissionsProgress.Count(_mission => _mission.Completed)}/{DataManager.Instance.PlayerData.MissionsProgress.Count}";
    }

    private string GetRefreshTime()
    {
        var _timeSpan = MissionManager.Instance.GetResetTime();
        if (_timeSpan.TotalHours >= 1)
        {
            return $"{(int)_timeSpan.TotalHours}h {_timeSpan.Minutes}m";
        }

        if (_timeSpan.TotalMinutes >= 1)
        {
            return $"{(int)_timeSpan.TotalMinutes}m";
        }

        if (_timeSpan.TotalSeconds>0)
        {
            return $"{(int)_timeSpan.TotalSeconds}s";
        }
        
        return "0s";
    }

    private void ShowLoginRewards()
    {
        foreach (var _reward in DataManager.Instance.GameData.LoginRewards.OrderBy(_mission => _mission.Days))
        {
            LoginProgressDisplay _rewardDisplay = Instantiate(loginProgressDisplay, progressHolder);
            bool _didUnlock = _reward.Days <= DataManager.Instance.PlayerData.WeeklyLoginAmount;
            _rewardDisplay.Setup(_didUnlock,_reward.Days);
        }
    }

    private void ShowMissions()
    {
        foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
        {
            MissionDisplay _missionDisplay = Instantiate(missionDisplay, missionHolder);
            _missionDisplay.Setup(_missionProgress);
        }
    }

    private void TryClaim(int _rewardNumber)
    {
        int _choseQoomon = DoTryClaim(_rewardNumber);
        if (_choseQoomon==-1)
        {
            return;
        }
        qoomonUnlockingPanel.Setup(_choseQoomon, () => SceneManager.Instance.ReloadScene());
    }

    public static int DoTryClaim(int _rewardNumber)
    {
        if (DataManager.Instance.PlayerData.WeeklyLoginAmount < _rewardNumber)
        {
            return -1;
        }

        if (DataManager.Instance.PlayerData.ClaimedLoginRewards.Contains(_rewardNumber))
        {
            return -1;
        }

        int _choseQoomon = DataManager.Instance.PlayerData.GetQoomonFromPool();
        DataManager.Instance.PlayerData.AddClaimedLoginReward(_rewardNumber);
        if (_choseQoomon != -1)
        {
            DataManager.Instance.PlayerData.AddQoomon(_choseQoomon);
            return _choseQoomon;
        }

        DataManager.Instance.PlayerData.Exp += 15;
        SceneManager.Instance.ReloadScene();

        return -1;
    }

}
