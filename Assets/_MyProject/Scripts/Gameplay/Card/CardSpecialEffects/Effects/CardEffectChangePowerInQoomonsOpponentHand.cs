using System.Collections.Generic;
using UnityEngine;

public class CardEffectChangePowerInQoomonsOpponentHand : CardEffectBase
{
    [SerializeField] private int power;

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
        ChangePower();
    }

    private void ChangePower()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _oppoentsCardsInHand = GameplayManager.Instance.OpponentPlayer.CurrentCardsInHand;

        foreach (var _card in _oppoentsCardsInHand)
        {
            _card.Stats.Power += power;
        }
    }
}
