using System.Collections.Generic;
using UnityEngine;

public class CardEffectGivePowerToDeck : CardEffectBase
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

        List<CardObject> _deckList = GameplayManager.Instance.MyPlayer.CurrentDeckCards;

        foreach (var _card in _deckList)
        {
            _card.Stats.Power += power;
        }
    }
}
