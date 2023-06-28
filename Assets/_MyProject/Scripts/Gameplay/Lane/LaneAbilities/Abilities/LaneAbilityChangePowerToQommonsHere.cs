using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityChangePowerToQommonsHere : LaneAbilityBase
{
    [SerializeField] private int powerAmount;

    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += EffectNewCards;
        EffectCardsAlreadyOnLane();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= EffectNewCards;
    }

    private void EffectCardsAlreadyOnLane()
    {
        List<CardObject> _myCards = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location);
        List<CardObject> _opponentCards = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location);

        ChangePower(_myCards);
        ChangePower(_opponentCards);

        void ChangePower(List<CardObject> _cards)
        {
            foreach (var _card in _cards)
            {
                _card.Stats.ChagePowerDueToLocation += powerAmount;
            }
        }
    }

    private void EffectNewCards(CardObject _cardObject)
    {
        if (_cardObject.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        _cardObject.Stats.ChagePowerDueToLocation += powerAmount;
    }
}
