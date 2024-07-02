using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    public static MissionPanel Instance;

    [SerializeField] private LoginProgressDisplay loginProgressDisplay;
    [SerializeField] private Transform progressHolder;
    [SerializeField] private Button close;
    [SerializeField] private QoomonUnlockingPanel qoomonUnlockingPanel;
    [SerializeField] private TextMeshProUGUI loggedInText;
    [SerializeField] private TextMeshProUGUI willRefreshInText;
    [SerializeField] private TextMeshProUGUI numberOfTasksCompleted;
    [SerializeField] private MissionDisplay missionDisplay;
    [SerializeField] private SeasonalTaskDisplay seasonalTaskDisplay;
    [SerializeField] private Transform missionHolder;
    [SerializeField] private Transform seasonalHolder;
    [SerializeField] private GameObject seasonalHeader;
    [SerializeField] private GameObject seasonalSeparator;

    public Button pwaOverlay;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (close)
        {
            close.onClick.AddListener(Close);
        }

        LoginProgressDisplay.OnClicked += TryClaim;
        MissionManager.OnClaimed += ShowCompletedText;

        if (pwaOverlay)
        {
            pwaOverlay.onClick.AddListener(ClosePwaOverlay);
        }
    }

    private void OnDisable()
    {
        if (close)
        {
            close.onClick.RemoveListener(Close);
        }

        LoginProgressDisplay.OnClicked -= TryClaim;
        MissionManager.OnClaimed += ShowCompletedText;

        if (pwaOverlay)
        {
            pwaOverlay.onClick.RemoveListener(ClosePwaOverlay);
        }
    }

    private void ShowCompletedText(MissionProgress _obj)
    {
        ShowDailyCompletedText();
    }

    private void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void ClosePwaOverlay()
    {
        pwaOverlay.gameObject.SetActive(false);
    }

    private void Start()
    {
        ShowLoginRewards();
        ShowDailyCompletedText();
        if (loggedInText)
        {
            loggedInText.text = $"{DataManager.Instance.PlayerData.WeeklyLoginAmount}/7 days";
        }

        ShowDailyCompletedText();
        ShowMissions();
        Debug.Log("aa");
        TryShowSeasonalTasks();
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        while (gameObject.activeSelf)
        {
            ShowDailyCompletedText();
            yield return new WaitForSeconds(1);
        }
    }

    private void ShowDailyCompletedText()
    {
        if (numberOfTasksCompleted == null)
        {
            return;
        }

        willRefreshInText.text = $"Will be refreshed in {GetRefreshTime()}";
        numberOfTasksCompleted.text =
            $"{DataManager.Instance.PlayerData.MissionsProgress.Count(_mission => _mission.Completed)}/{DataManager.Instance.PlayerData.MissionsProgress.Count}";
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

        if (_timeSpan.TotalSeconds > 0)
        {
            return $"{(int)_timeSpan.TotalSeconds}s";
        }

        return "0s";
    }

    private void ShowLoginRewards()
    {
        if (progressHolder == null)
        {
            return;
        }

        foreach (var _reward in DataManager.Instance.GameData.LoginRewards.OrderBy(_mission => _mission.Days))
        {
            LoginProgressDisplay _rewardDisplay = Instantiate(loginProgressDisplay, progressHolder);
            bool _didUnlock = _reward.Days <= DataManager.Instance.PlayerData.WeeklyLoginAmount;
            _rewardDisplay.Setup(_didUnlock, _reward.Days);
        }
    }

    private void ShowMissions()
    {
        if (missionDisplay == null)
        {
            return;
        }

        foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
        {
            MissionDisplay _missionDisplay = Instantiate(missionDisplay, missionHolder);
            _missionDisplay.Setup(_missionProgress);
        }
    }

    private void TryShowSeasonalTasks()
    {
        if (seasonalTaskDisplay == null)
        {
            Debug.Log(1);
            return;
        }

        Debug.Log(2);

        foreach (SeasonalTaskType _seasonalTask in Enum.GetValues(typeof(SeasonalTaskType)))
        {
            if (JavaScriptManager.Instance.IsPwaPlatform && _seasonalTask == SeasonalTaskType.Pwa)
            {
                continue;
            }

            if (JavaScriptManager.Instance.IsTelegramPlatform && _seasonalTask == SeasonalTaskType.Pwa)
            {
                continue;
            }
            
            if (!DataManager.Instance.PlayerData.IsGuest && _seasonalTask == SeasonalTaskType.SocialAccount)
            {
                continue;
            }

            SeasonalTaskDisplay _seasonalTaskDisplay = Instantiate(seasonalTaskDisplay, seasonalHolder);
            _seasonalTaskDisplay.Setup(_seasonalTask);
        }

        Debug.Log(3);
        if (seasonalHolder.childCount == 0)
        {
            Debug.Log(4);
            SetSeasonalElementsActive(false);
        }
    }

    private void SetSeasonalElementsActive(bool _isActive)
    {
        if (seasonalHeader == null)
        {
            return;
        }

        seasonalHeader.SetActive(_isActive);
        seasonalHolder.gameObject.SetActive(_isActive);
        seasonalSeparator.SetActive(_isActive);
    }

    private void TryClaim(int _rewardNumber)
    {
        int _choseQoomon = DoTryClaim(_rewardNumber);
        if (_choseQoomon == -1)
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