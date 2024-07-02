
public class LaneAbilityDisableOnGoingEffects : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.GlobalAmountOfOngoingEffects = 0;
    }
}
