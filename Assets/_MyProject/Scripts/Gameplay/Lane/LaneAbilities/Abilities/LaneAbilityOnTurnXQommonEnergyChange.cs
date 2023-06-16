using UnityEngine;

public class LaneAbilityOnTurnXQommonEnergyChange : LaneAbilityBase
{
    [SerializeField] int round;
    [SerializeField] int change;

    public override void Subscribe()
    {
        GameplayManager.UpdatedRound += ManageAbility;
    }

    void ManageAbility()
    {
        int _currentRound = GameplayManager.Instance.CurrentRound;
        if (_currentRound == round)
        {
            laneDisplay.AbilityShowAsActive();
            GameplayManager.Instance.UpdateQommonCosts(change);
        }
        else if (_currentRound == round + 1)
        {
            laneDisplay.AbilityShowAsInactive();
            GameplayManager.Instance.UpdateQommonCosts(-change);
        }
    }
}
