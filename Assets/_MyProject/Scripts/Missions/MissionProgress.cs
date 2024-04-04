using System;

[Serializable]
public class MissionProgress
{
    public static Action<int> UpdatedProgress;
    public int Id;
    public int Value;
    public bool Claimed;
    public bool IsHard;
    
    public bool Completed
    {
        get
        {
            MissionData _challengeData = DataManager.Instance.GameData.Missions.Find(_mission => _mission.Id == Id);
            return IsHard
                ?_challengeData.Hard.AmountNeeded - Value <= 0
                : _challengeData.Normal.AmountNeeded - Value <= 0;
        }
    }

    public void IncreaseAmount()
    {
        if (Completed)
        {
            return;
        }

        Value++;
        UpdatedProgress?.Invoke(Id);
    }
}
