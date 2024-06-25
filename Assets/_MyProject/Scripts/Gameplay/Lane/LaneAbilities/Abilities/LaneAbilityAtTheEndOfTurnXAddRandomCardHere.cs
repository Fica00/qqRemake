using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaneAbilityAtTheEndOfTurnXAddRandomCardHere : LaneAbilityBase
{
    [SerializeField] private int round;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.OnFinishedGameplayLoop += CheckForRound;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.OnFinishedGameplayLoop -= CheckForRound;
    }

    private void CheckForRound()
    {
        if (GameplayManager.Instance.CurrentRound != round)
        {
            return;
        }

        AddRandomCard();
    }

    private void AddRandomCard()
    {
        //GameplayManager.Instance.TableHandler.
    }
}
