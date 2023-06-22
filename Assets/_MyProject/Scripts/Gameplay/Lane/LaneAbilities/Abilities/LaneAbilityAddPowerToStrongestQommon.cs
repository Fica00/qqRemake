using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityAddPowerToStrongestQommon : LaneAbilityBase
{
    [SerializeField] int powerToAdd;
    CardObject strongestCard;

    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckCard;
        CheckCards();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCard;
    }

    void CheckCard(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CheckCards();
    }


    void CheckCards()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject> _opponentCardsOLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);

        CardObject _strongestCard = null;
        if (_myCardsOnLane.Count > 0)
        {
            _strongestCard = _myCardsOnLane[0];
        }
        else if (_opponentCardsOLane.Count > 0)
        {
            _strongestCard = _opponentCardsOLane[0];
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
            return;
        }

        if (strongestCard != null)
        {
            strongestCard.Stats.ChagePowerDueToLocation -= powerToAdd;
            strongestCard = null;
        }
        laneDisplay.AbilityShowAsInactive();

        _strongestCard = GetStrongestCard(_strongestCard, _myCardsOnLane);
        _strongestCard = GetStrongestCard(_strongestCard, _opponentCardsOLane);

        if (!IsThereCardWithSamePower(_strongestCard, _strongestCard.IsMy ? _opponentCardsOLane : _myCardsOnLane))
        {
            laneDisplay.AbilityShowAsActive();
            strongestCard = _strongestCard;
            strongestCard.Stats.ChagePowerDueToLocation += powerToAdd;
        }

    }

    CardObject GetStrongestCard(CardObject _currentStrongest, List<CardObject> _cards)
    {
        foreach (var _card in _cards)
        {
            if (_currentStrongest.Stats.Power < _card.Stats.Power)
            {
                _currentStrongest = _card;
            }
        }

        return _currentStrongest;
    }

    bool IsThereCardWithSamePower(CardObject _strongestCard, List<CardObject> _cards)
    {
        foreach (var _card in _cards)
        {
            if (_strongestCard.Stats.Power == _card.Stats.Power)
            {
                return true;
            }
        }

        return false;
    }
}
