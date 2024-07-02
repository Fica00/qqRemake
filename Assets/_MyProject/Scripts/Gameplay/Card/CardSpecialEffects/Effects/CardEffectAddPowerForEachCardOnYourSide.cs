using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerForEachCardOnYourSide : CardEffectBase
{
    [SerializeField] private int powerToAdd;

    public override void Subscribe()
    {
        CountCards();
    }

    private void CountCards()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);
        int _amountOfCardsOnLane = _myCardsOnLane.Count;
        int _powerToAdd = _amountOfCardsOnLane * powerToAdd;
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy); 
        _i++)
        {
            cardObject.Stats.Power += _powerToAdd;
        }

        foreach (var _cardOnLane in _myCardsOnLane)
        {
            LanePlaceIdentifier _placeIdentifier = _cardOnLane.GetComponentInParent<LanePlaceIdentifier>();
            GameplayManager.Instance.FlashLocation(_placeIdentifier.Id,Color.white,3);
        }
    }
}
