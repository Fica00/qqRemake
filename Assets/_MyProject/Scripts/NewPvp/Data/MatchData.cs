using System;
using System.Collections.Generic;

[Serializable]
public class MatchData
{
    public List<string> Players;
    public bool IsMasterClient => Players[0] == FirebaseManager.Instance.PlayerId;
    public string RoomName;
}
