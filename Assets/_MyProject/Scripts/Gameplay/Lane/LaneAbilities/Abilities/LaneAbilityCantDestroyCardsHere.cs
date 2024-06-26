public class LaneAbilityCantDestroyCardsHere : LaneAbilityBase
{
    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.CanRemoveCards = false;
    }
}