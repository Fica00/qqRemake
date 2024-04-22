using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityAddPowerToStrongestQommon : LaneAbilityBase
{
    [SerializeField] private int powerToAdd;
    private List<CardObject> strongestCards = new ();

    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += CheckCard;
        CheckCards();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckCard;
    }

    private void CheckCard(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CheckCards();
    }


    private void CheckCards()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject> _opponentCardsOLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);
        
        int _strongestPower = int.MinValue;
        foreach (var _card in _myCardsOnLane)
        {
            if (_strongestPower<_card.Stats.Power)
            {
                _strongestPower = _card.Stats.Power;
            }
        }
        
        foreach (var _card in _opponentCardsOLane)
        {
            if (_strongestPower<_card.Stats.Power)
            {
                _strongestPower = _card.Stats.Power;
            }
        }

        if (_strongestPower == int.MinValue)
        {
            foreach (var _strongestCard in strongestCards)
            {
                _strongestCard.Stats.ChagePowerDueToLocation -= powerToAdd;
            }

            strongestCards.Clear();
            return;
        }

        if (strongestCards.Count>0 && _strongestPower != strongestCards[0].Stats.Power)
        {
            foreach (var _strongestCard in strongestCards)
            {
                _strongestCard.Stats.ChagePowerDueToLocation -= powerToAdd;
            }
            
            strongestCards.Clear();
        }
        
        List<CardObject> _strongestCards = new ();
        foreach (var _card in GetStrongestCards(_myCardsOnLane, _strongestPower))
        {
            _strongestCards.Add(_card);
        }
        foreach (var _card in GetStrongestCards(_opponentCardsOLane, _strongestPower))
        {
            _strongestCards.Add(_card);
        }

        foreach (var _strongestCard in _strongestCards)
        {
            if (strongestCards.Contains(_strongestCard))
            {
                continue;
            }
            
            _strongestCard.Stats.ChagePowerDueToLocation += powerToAdd;
            strongestCards.Add(_strongestCard);
        }
    }

    private List<CardObject> GetStrongestCards(List<CardObject> _cards, float _power)
    {
        List<CardObject> _validCards = new();
        foreach (var _card in _cards)
        {
            if (_power != _card.Stats.Power)
            {
                continue;
            }
            
            _validCards.Add(_card);
        }

        return _validCards;
    }
}
