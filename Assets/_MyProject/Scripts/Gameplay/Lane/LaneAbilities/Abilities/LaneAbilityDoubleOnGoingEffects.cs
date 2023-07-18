using UnityEngine;

public class LaneAbilityDoubleOnGoingEffects : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.AmountOfOngoingEffects *= 2;
    }
}
