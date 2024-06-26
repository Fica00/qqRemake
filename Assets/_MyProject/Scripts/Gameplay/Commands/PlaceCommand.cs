using System;

[Serializable]
public class PlaceCommand
{
    public CardObject Card;
    public int PlaceId;
    public bool IsMyPlayer;
    public LaneLocation Location;
    public int Power;
    public int Energy;
}