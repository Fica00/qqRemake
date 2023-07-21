using System.Collections.Generic;
using UnityEngine;

public class CardEffectYourNCostQommonsHereGetMPower : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    [SerializeField] private int cost;
    
    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckCard;
        CheckQommonsThatAreAlreadyHere();
    }

    private void CheckQommonsThatAreAlreadyHere()
    {
        List<CardObject> _cardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);

        foreach (var _cardOnLane in _cardsOnLane)
        {
            if (_cardOnLane.Stats.Energy!=cost)
            {
                continue;
            }
            
            for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
            {
                _cardOnLane.Stats.Power += amountOfPower;
            }
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
        
        if (_card.Stats.Energy!=cost)
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
            _card.Display.EnlargedPowerAnimation(cardObject.IsMy);
        }
    }
}
