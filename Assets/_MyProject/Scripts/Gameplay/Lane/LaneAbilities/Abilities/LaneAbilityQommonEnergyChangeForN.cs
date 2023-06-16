using UnityEngine;

public class LaneAbilityQommonEnergyChangeForN : LaneAbilityBase
{
    [SerializeField] int change;

    public override void Subscribe()
    {
        laneDisplay.AbilityShowAsActive();
        GameplayManager.Instance.UpdateQommonCosts(change);
    }
}
