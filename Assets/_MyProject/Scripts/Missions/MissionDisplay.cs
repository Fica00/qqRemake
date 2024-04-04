using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDisplay : MonoBehaviour
{
    public static Action<MissionProgress> OnClaimPressed;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject claimHolder;
    [SerializeField] private Image rewardDisplay;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private GameObject completed;

    private MissionProgress missionProgress;
    
    public void Setup(MissionProgress _progress)
    {
        completed.SetActive(false);
        claimHolder.SetActive(false);
        missionProgress = _progress;
        MissionData _missionData = DataManager.Instance.GameData.GetMission(_progress.Id);
        MissionTaskData _taskData = _progress.IsHard ? _missionData.Hard : _missionData.Normal;
        if (_progress.Completed)
        {
            if (_progress.Claimed)
            {
                completed.SetActive(true);
            }
            else
            {
                claimHolder.SetActive(true);
            }
            descDisplay.text = string.Empty;
            progressDisplay.text = string.Empty;
        }
        else
        {
            descDisplay.text = _taskData.Description;
            progressDisplay.text = $"{_progress.Value}/{_taskData.AmountNeeded}";
        }

        rewardDisplay.sprite = _taskData.RewardType == ItemType.Qoomon
            ? SpriteProvider.Instance.GetQoomonSprite(_taskData.RewardAmount)
            : SpriteProvider.Instance.Get(_taskData.RewardType);
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(Claim);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(Claim);
    }

    private void Claim()
    {
        OnClaimPressed?.Invoke(missionProgress);
        Setup(missionProgress);
    }
}
