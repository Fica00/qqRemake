using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerForEachCardOnOppositeSide : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    private int amountOfCountedCards;

    public override void Subscribe()
    {
        amountOfCountedCards = 0;
        TableHandler.OnRevealdCard += TryToAddPower;
        CountCards();
    }

    private void OnDestroy()
    {
        TableHandler.OnRevealdCard -= TryToAddPower;
    }

    private void TryToAddPower(CardObject _cardObject)
    {
        if (cardObject.IsMy == _cardObject.IsMy)
        {
            return;
        }

        if (cardObject.LaneLocation != _cardObject.LaneLocation)
        {
            return;
        }

        CountCards();
    }

    private void CountCards()
    {
        List<CardObject> _opponentsCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);
        int _amountOfCardsOnLane = _opponentsCardsOnLane.Count;
        int _difference = _amountOfCardsOnLane - amountOfCountedCards;
        int _powerToAdd = _difference * powerToAdd;
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; i++)
        {
            cardObject.Stats.Power += _powerToAdd;
        }
        amountOfCountedCards = _amountOfCardsOnLane;
    }
}
