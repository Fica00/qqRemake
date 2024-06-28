using System.Collections.Generic;

public class LaneAbilityDestroyWeakestQoomonHereEachRound : LaneAbilityBase
{
    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.OnFinishedGameplayLoop += DestroyWeakest;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.OnFinishedGameplayLoop -= DestroyWeakest;
    }

    private void DestroyWeakest()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject> _opponentCardsOLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);

        List<CardObject> _allCardsOnLane = new List<CardObject>();

        _allCardsOnLane.AddRange(_myCardsOnLane);
        _allCardsOnLane.AddRange(_opponentCardsOLane);

        if (_allCardsOnLane.Count <= 1)
        {
            return;
        }

        List<CardObject> _weakestCards = new List<CardObject>();
        _weakestCards.Add(_myCardsOnLane[0]);

        for (int i = 1; i < _allCardsOnLane.Count; i++)
        {
            if (_weakestCards[0].Stats.Power > _allCardsOnLane[i].Stats.Power)
            {
                _weakestCards.Clear();
                _weakestCards.Add(_allCardsOnLane[i]);
            }
            else if(_weakestCards[0].Stats.Power == _allCardsOnLane[i].Stats.Power)
            {
                _weakestCards.Add(_allCardsOnLane[i]);
            }
        }

        foreach (var _card in _weakestCards)
        {
            GameplayManager.Instance.MyPlayer.DestroyCardFromTable(_card);
        }
    }
}