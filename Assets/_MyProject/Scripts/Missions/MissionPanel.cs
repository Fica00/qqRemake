using System.Collections.Generic;
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

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        LoginProgressDisplay.OnClicked += TryClaim;
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
        LoginProgressDisplay.OnClicked -= TryClaim;
    }

    private void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void Start()
    {
        ShowLoginRewards();
        loggedInText.text = $"{DataManager.Instance.PlayerData.WeeklyLoginAmount}/7";
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

    private void TryClaim(int _rewardNumber)
    {
        if (DataManager.Instance.PlayerData.WeeklyLoginAmount < _rewardNumber)
        {
            return;
        }

        if (DataManager.Instance.PlayerData.ClaimedLoginRewards.Contains(_rewardNumber))
        {
            return;
        }

        List<int> _possibleQoomons = new List<int>()
        {
            43,
            42,
            41,
            40,
            39,
            38,
            32,
            31,
            30
        };
        int _randomQoomon = _possibleQoomons[Random.Range(0, _possibleQoomons.Count)];
        DataManager.Instance.PlayerData.AddQoomon(_randomQoomon);
        DataManager.Instance.PlayerData.AddClaimedLoginReward(_rewardNumber);
        qoomonUnlockingPanel.Setup(_randomQoomon, () => SceneManager.Instance.ReloadScene());
    }

}
