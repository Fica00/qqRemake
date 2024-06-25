using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddManaToRandomQoomonInOpponentHand : CardEffectBase
{
    [SerializeField] private int manaToAdd;
    [SerializeField] private int manaLessThan;

    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            AddManaToRandom();
        }
    }

    private void AddManaToRandom()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        GameplayManager.Instance.ChangeInOpponentHandRandomCardEnergy(manaLessThan, manaToAdd, GameplayManager.Instance.OpponentPlayer);
    }
}