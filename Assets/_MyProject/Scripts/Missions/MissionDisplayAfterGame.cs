using UnityEngine;

public class MissionDisplayAfterGame : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private MissionDisplay missionDisplay;
    [SerializeField] private Transform missionHolder;

    public void Setup()
    {
        holder.SetActive(true);
        ShowMissions();
    }
    
    private void ShowMissions()
    {
        foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
        {
            MissionDisplay _missionDisplay = Instantiate(missionDisplay, missionHolder);
            _missionDisplay.Setup(_missionProgress);
        }
    }
}
