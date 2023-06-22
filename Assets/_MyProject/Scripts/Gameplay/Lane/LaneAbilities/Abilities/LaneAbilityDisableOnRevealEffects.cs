public class LaneAbilityDisableOnRevealEffects : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.AmountOfRevealEffects = 0;
        laneDisplay.AbilityShowAsActive();
    }
}
