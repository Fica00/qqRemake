using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityXRoundQommonsCantBePlaced : LaneAbilityBase
{
    [SerializeField] private List<int> rounds;

    public override void Subscribe()
    {
        foreach (var _round in rounds)
        {
            laneDisplay.LaneSpecifics.CantPlaceCommonsOnRound.Add(_round);
        }

        GameplayManager.UpdatedRound += CheckRound;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= CheckRound;
    }

    void CheckRound()
    {
        if (rounds.Contains(GameplayManager.Instance.CurrentRound))
        {
            laneDisplay.AbilityShowAsActive();
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }
    }
}
