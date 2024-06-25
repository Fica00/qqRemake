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

        if (GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects < 1)
        {
            return;
        }

        AddManaToRandom();
    }

    private void AddManaToRandom()
    {
        List<CardObject> _opponentsCardsInHand = GameplayManager.Instance.OpponentPlayer.CurrentCardsInHand;

        if (_opponentsCardsInHand.Count == 0)
        {
            return;
        }

        CardObject _randomCardInHand = _opponentsCardsInHand.OrderBy(_ => Guid.NewGuid()).First(_qoomon => _qoomon.Stats.Energy < manaLessThan);

        if (_randomCardInHand == null)
        {
            return;
        }

        GameplayManager.Instance.ChangeCardEnergy(_randomCardInHand, manaToAdd);
    }
}