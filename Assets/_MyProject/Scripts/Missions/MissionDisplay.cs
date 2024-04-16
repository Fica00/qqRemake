using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDisplay : MonoBehaviour
{
    public static Action<MissionProgress> OnClaimPressed;
    [SerializeField] private Button claimButton;
    [SerializeField] private Sprite claim;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image rewardDisplay;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private Image progressFill;

    private MissionProgress missionProgress;

    public void Setup(MissionProgress _progress)
    {
        missionProgress = _progress;
        MissionData _missionData = DataManager.Instance.GameData.GetMission(_progress.Id);
        MissionTaskData _taskData = _progress.IsHard ? _missionData.Hard : _missionData.Normal;
        if (_progress.Completed)
        {
            if (!_progress.Claimed)
            {
                backgroundImage.sprite = claim;
            }
            
            progressDisplay.text = "Claim";
            progressFill.fillAmount = 1;
        }
        else
        {
            progressDisplay.text = $"{_progress.Value}/{_taskData.AmountNeeded}";
            progressFill.fillAmount = _progress.Value == 0 ? 0 : (float)_progress.Value / _taskData.AmountNeeded;
        }
        
        descDisplay.text = _taskData.Description;
        
        if (_progress.Claimed)
        {
            descDisplay.text = "Claimed";
            progressDisplay.text = string.Empty;
        }


        if (_taskData.RewardType == ItemType.Qoomon)
        {
            rewardDisplay.sprite = SpriteProvider.Instance.GetQoomonSprite(_taskData.RewardAmount);
        }
        else
        {
            rewardText.text = $"{_taskData.RewardAmount}{Utils.GetItemName(_taskData.RewardType)}";
        }
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
