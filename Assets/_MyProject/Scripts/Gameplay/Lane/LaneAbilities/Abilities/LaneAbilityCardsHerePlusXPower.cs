using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityCardsHerePlusXPower : LaneAbilityBase
{
    [SerializeField] private int powerToAdd;

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

        List<CardObject> _allCardsOnLane = new List<CardObject>();

        _allCardsOnLane.AddRange( _myCardsOnLane );
        _allCardsOnLane.AddRange(_opponentCardsOLane );

        foreach (var _card in _allCardsOnLane)
        {
            _card.Stats.Power += powerToAdd;
        }
    }
}