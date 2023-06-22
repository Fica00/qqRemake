using UnityEngine;

public class LaneAbilityOnlyXQommonsCanBePlacedHere : LaneAbilityBase
{
    [SerializeField] int amountOfQommons;
    public int AmountOfQommons => amountOfQommons;

    public override void Subscribe()
    {
        laneDisplay.AbilityShowAsActive();
        laneDisplay.LaneSpecifics.MaxAmountOfQommons = amountOfQommons;
    }
}
