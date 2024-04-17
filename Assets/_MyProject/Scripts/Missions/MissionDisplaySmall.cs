using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDisplaySmall : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private Image progressFill;
    
    public void Setup(MissionProgress _progress)
    {
        MissionData _missionData = DataManager.Instance.GameData.GetMission(_progress.Id);
        MissionTaskData _taskData = _progress.IsHard ? _missionData.Hard : _missionData.Normal;
        if (_progress.Completed)
        {
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
    }
}
