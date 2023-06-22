using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityOnTurnXReturnQommonsToDeckAndDrawN : LaneAbilityBase
{
    [SerializeField] int round;
    [SerializeField] int amountOfCards;
    bool isSubscribed = false;

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

    void CheckRound()
    {
        if (GameplayManager.Instance.CurrentRound == round)
        {
            laneDisplay.AbilityShowAsActive();

            GameplayManager.Instance.MyPlayer.ReturnCardsToDeck();

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
            isSubscribed = false;
            GameplayManager.UpdatedRound -= CheckRound;
            laneDisplay.AbilityShowAsInactive();
        }
    }
}
