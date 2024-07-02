public class LaneAbilityDoubleOnGoingEffects : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.GlobalAmountOfOngoingEffects *= 2;
    }
}
