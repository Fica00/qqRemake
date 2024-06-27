
public class LaneAbilityDisableOnGoingEffects : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.AmountOfOngoingEffects = 0;
    }
}
