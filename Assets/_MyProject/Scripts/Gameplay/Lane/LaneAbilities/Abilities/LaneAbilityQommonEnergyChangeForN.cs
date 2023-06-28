using UnityEngine;

public class LaneAbilityQommonEnergyChangeForN : LaneAbilityBase
{
    [SerializeField] private int change;

    public override void Subscribe()
    {
        GameplayManager.Instance.UpdateQommonCosts(change);
    }
}
