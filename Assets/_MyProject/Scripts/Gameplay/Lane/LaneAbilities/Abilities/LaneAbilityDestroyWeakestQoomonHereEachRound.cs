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

        CardObject _weakestCard = _allCardsOnLane[0];

        for (int i = 1; i < _allCardsOnLane.Count; i++)
        {
            if (_weakestCard.Stats.Power > _allCardsOnLane[i].Stats.Power) 
            {
                _weakestCard = _allCardsOnLane[i];
            }
        }

        GameplayManager.Instance.MyPlayer.DestroyCardFromTable(_weakestCard);
        // Dodaj za multiplayer u slucaju da ima vise karata sa istim powerom, mora jedna samo da se unisti
    }
}
