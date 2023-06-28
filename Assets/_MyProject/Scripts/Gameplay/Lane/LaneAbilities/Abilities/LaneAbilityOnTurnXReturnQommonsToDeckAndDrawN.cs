using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityOnTurnXReturnQommonsToDeckAndDrawN : LaneAbilityBase
{
    [SerializeField] private int round;
    [SerializeField] private int amountOfCards;
    private bool isSubscribed = false;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckRound;
    }

    private void OnDisable()
    {
        if (isSubscribed)
        {
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }

    private void CheckRound()
    {
        if (GameplayManager.Instance.CurrentRound == round)
        {
            GameplayManager.Instance.MyPlayer.ReturnCardsToDeck();
            laneDisplay.AbilityShowAsActive();

            if (!GameplayManager.IsPvpGame)
            {
                GameplayManager.Instance.OpponentPlayer.ReturnCardsToDeck();
            }

            for (int i = 0; i < amountOfCards; i++)
            {
                GameplayManager.Instance.DrawCard();
            }
        }
        else if (GameplayManager.Instance.CurrentRound > round)
        {
            laneDisplay.AbilityShowAsInactive();
            isSubscribed = false;
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }
}
