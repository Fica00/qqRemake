using System.Collections.Generic;
using UnityEngine;

public class AddPowerForEachCardOnOppositeSide : CardSpecialEffectBase
{
    [SerializeField] int powerToAdd;
    int amountOfCountedCards;

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

    void TryToAddPower(CardObject _cardObject)
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

    void CountCards()
    {
        List<CardObject> _opponentsCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);
        int _amountOfCardsOnLane = _opponentsCardsOnLane.Count;
        int _difference = _amountOfCardsOnLane - amountOfCountedCards;
        int _powerToAdd = _difference * powerToAdd;
        cardObject.Stats.Power += _powerToAdd;
        amountOfCountedCards = _amountOfCardsOnLane;
    }
}
