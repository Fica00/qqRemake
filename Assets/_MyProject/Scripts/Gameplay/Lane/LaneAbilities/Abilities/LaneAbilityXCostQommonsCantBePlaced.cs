using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityXCostQommonsCantBePlaced : LaneAbilityBase
{
    [SerializeField] private List<int> qommonCosts;

    public override void Subscribe()
    {
        foreach (var _qommonCost in qommonCosts)
        {
            laneDisplay.LaneSpecifics.CantPlaceCommonsThatCost.Add(_qommonCost);
        }
    }
}
