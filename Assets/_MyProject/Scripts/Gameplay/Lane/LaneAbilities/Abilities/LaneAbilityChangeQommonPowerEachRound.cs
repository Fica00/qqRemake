using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityChangeQommonPowerEachRound : LaneAbilityBase
{
    [SerializeField] int amountOfPower;

    public override void Subscribe()
    {
        laneDisplay.AbilityShowAsActive();
        GameplayManager.UpdatedRound += ChangePower;
    }

    void ChangePower()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject>  _opponentCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);

        ChangePower(_myCardsOnLane);
        ChangePower(_opponentCardsOnLane);

        void ChangePower(List<CardObject> _cards)
        {
            foreach (var _card in _cards)
            {
                _card.Stats.ChagePowerDueToLocation += amountOfPower;
            }
        }
    }
}
