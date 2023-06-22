using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityAddCopyOfAQommonToAnotherLocation : LaneAbilityBase
{
    List<CardObject> qommons = new List<CardObject>();
    public override void Subscribe()
    {
        laneDisplay.AbilityShowAsActive();
        TableHandler.OnRevealdCard += AddCopyOfAQommon;
        GameplayManager.UpdatedRound += PlaceQommons;
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= AddCopyOfAQommon;
        GameplayManager.UpdatedRound -= PlaceQommons;
    }

    void AddCopyOfAQommon(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CardObject _copyOfCard = CardsManager.Instance.CreateCard(_card.Details.Id, _card.IsMy);
        qommons.Add(_copyOfCard);
    }

    void PlaceQommons()
    {
        foreach (var _qommon in qommons)
        {
            LaneDisplay _choosendLane = null;
            for (int i = 0; i < GameplayManager.Instance.Lanes.Count; i++)
            {
                if (i == (int)laneDisplay.Location)
                {
                    continue;
                }
                if (GameplayManager.Instance.Lanes[i].GetPlaceLocation(_qommon.IsMy) != null)
                {
                    bool _shouldSkip = false;
                    foreach (var _laneEffect in GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[i]].Abilities)
                    {
                        if (_laneEffect is LaneAbilityOnlyXQommonsCanBePlacedHere)
                        {
                            _shouldSkip = true;
                            break;
                        }
                    }
                    if (_shouldSkip)
                    {
                        continue;
                    }
                    _choosendLane = GameplayManager.Instance.Lanes[i];
                    break;
                }
            }

            if (_choosendLane == null)
            {
                return;
            }

            if (GameplayManager.IsPvpGame && !_qommon.IsMy)
            {
                return;
            }

            _qommon.ForcePlace(_choosendLane);
        }

        qommons.Clear();
    }
}
