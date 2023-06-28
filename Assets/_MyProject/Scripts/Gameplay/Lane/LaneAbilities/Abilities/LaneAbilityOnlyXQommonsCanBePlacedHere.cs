using UnityEngine;

public class LaneAbilityOnlyXQommonsCanBePlacedHere : LaneAbilityBase
{
    [SerializeField] private int amountOfQommons;
    public int AmountOfQommons => amountOfQommons;

    public override void Subscribe()
    {
        laneDisplay.LaneSpecifics.MaxAmountOfQommons = amountOfQommons;
    }
}
