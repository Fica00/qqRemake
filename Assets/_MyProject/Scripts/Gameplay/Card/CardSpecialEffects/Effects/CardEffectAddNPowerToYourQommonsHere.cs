using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddNPowerToYourQommonsHere : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    
    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += CheckCard;
        CheckQommonsThatAreAlreadyHere();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCard;
    }

    private void CheckQommonsThatAreAlreadyHere()
    {
        List<CardObject> _cardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);

        foreach (var _cardOnLane in _cardsOnLane)
        {
            for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
            {
                _cardOnLane.Stats.Power += amountOfPower;
            }
            _cardOnLane.Display.EnlargedPowerAnimation(_cardOnLane.IsMy);
        }
    }

    private void CheckCard(CardObject _card)
    {
        if (_card.IsMy!=cardObject.IsMy)
        {
            return;
        }

        if (_card.LaneLocation!=cardObject.LaneLocation)
        {
            return;
        }

        if (_card==cardObject)
        {
            return;
        }
        
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            _card.Stats.Power += amountOfPower;
        }
        
        _card.Display.EnlargedPowerAnimation(_card.IsMy);
    }
}
