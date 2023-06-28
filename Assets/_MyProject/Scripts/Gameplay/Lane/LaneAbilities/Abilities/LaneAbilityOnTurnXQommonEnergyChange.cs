using UnityEngine;

public class LaneAbilityOnTurnXQommonEnergyChange : LaneAbilityBase
{
    [SerializeField] private int round;
    [SerializeField] private int change;

    public override void Subscribe()
    {
        GameplayManager.UpdatedRound += ManageAbility;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= ManageAbility;
    }

    private void ManageAbility()
    {
        int _currentRound = GameplayManager.Instance.CurrentRound;
        if (_currentRound == round)
        {
            laneDisplay.AbilityShowAsActive();
            GameplayManager.Instance.UpdateQommonCosts(change);
        }
        else if (_currentRound > round)
        {
            laneDisplay.AbilityShowAsInactive();
            GameplayManager.Instance.UpdateQommonCosts(-change);
        }
    }
}
