using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityChangeQommonPowerEachRound : LaneAbilityBase
{
    [SerializeField] private int amountOfPower;

    public override void Subscribe()
    {
        GameplayManager.UpdatedRound += ChangePower;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= ChangePower;
    }

    private void ChangePower()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject>  _opponentCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);

        ChangePower(_myCardsOnLane);
        ChangePower(_opponentCardsOnLane);

        void ChangePower(List<CardObject> _cards)
        {
            if (_cards == null)
            {
                return;
            }
            foreach (var _card in _cards)
            {
                _card.Stats.ChagePowerDueToLocation += amountOfPower;
            }
        }
    }
}
