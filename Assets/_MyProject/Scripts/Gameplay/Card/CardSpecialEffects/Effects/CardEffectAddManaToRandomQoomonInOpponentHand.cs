using System.Collections.Generic;
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
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _opponentsCardsInHand = GameplayManager.Instance.OpponentPlayer.CurrentCardsInHand;

        var randomCardInHand = Random.Range(0, _opponentsCardsInHand.Count);

        CardObject randomCard = _opponentsCardsInHand[randomCardInHand];

        if (randomCard.Stats.Energy < manaLessThan)
        {
            randomCard.Stats.Energy += manaToAdd;
        }
    }
}
