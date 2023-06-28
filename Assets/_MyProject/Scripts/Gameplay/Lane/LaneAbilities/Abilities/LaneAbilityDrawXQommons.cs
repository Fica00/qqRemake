using System;
using System.Collections;
using UnityEngine;

public class LaneAbilityDrawXQommons : LaneAbilityBase
{
    [SerializeField] private int amountOfCardsToDraw;
    private bool shouldHide;
    public override void Subscribe()
    {
        for (int i = 0; i < amountOfCardsToDraw; i++)
        {
            GameplayManager.Instance.DrawCard();
        }
        GameplayManager.UpdatedRound += CheckRound;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= CheckRound;
    }

    void CheckRound()
    {
        if (shouldHide)
        {
            laneDisplay.AbilityShowAsActive();
        }

        shouldHide = true;
    }
}
