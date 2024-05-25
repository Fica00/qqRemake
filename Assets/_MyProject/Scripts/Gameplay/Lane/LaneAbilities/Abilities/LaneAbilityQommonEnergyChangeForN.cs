using UnityEngine;

public class LaneAbilityQommonEnergyChangeForN : LaneAbilityBase
{
    [SerializeField] private int change;
    [HideInInspector] public bool IsActive;
    public int Change => change;

    public override void Subscribe()
    {
        GameplayManager.Instance.UpdateQommonCosts(change);
        IsActive = true;
    }

    private void OnDisable()
    {
        IsActive = false;
    }
}
