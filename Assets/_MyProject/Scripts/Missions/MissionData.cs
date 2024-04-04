using System;

[Serializable]
public class MissionData
{
    public int Id;
    public MissionType Type;
    public MissionTaskData Normal;
    public MissionTaskData Hard;
}