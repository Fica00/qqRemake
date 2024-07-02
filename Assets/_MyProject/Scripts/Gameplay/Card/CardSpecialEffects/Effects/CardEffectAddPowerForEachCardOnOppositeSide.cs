using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerForEachCardOnOppositeSide : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    private int amountOfCountedCards;

    public override void Subscribe()
    {
        isSubscribed = true;
        amountOfCountedCards = 0;
        TableHandler.OnRevealdCard += TryToAddPower;
        LaneSpecifics.UpdatedAmountOfOngoingEffects += CountCards;
        CountCards();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= TryToAddPower;
        LaneSpecifics.UpdatedAmountOfOngoingEffects -= CountCards;
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
        Debug.Log("Amount of cards on the lane: "+_amountOfCardsOnLane);
        Debug.Log("Amount of cards in previous count: "+amountOfCountedCards);
        int _difference = _amountOfCardsOnLane - amountOfCountedCards;
        int _powerToAdd = _difference * powerToAdd;
        Debug.Log("Difference "+_difference);
        Debug.Log("Power to add "+_powerToAdd);
        Debug.Log("Total power to add "+_powerToAdd*GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy));
        Debug.Log(GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy));
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy); _i++)
        {
            Debug.Log(1);
            cardObject.Stats.Power += _powerToAdd;
        }
        amountOfCountedCards = _amountOfCardsOnLane;
    }
}
