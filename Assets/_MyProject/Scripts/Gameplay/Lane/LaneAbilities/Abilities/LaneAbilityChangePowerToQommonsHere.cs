using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityChangePowerToQommonsHere : LaneAbilityBase
{
    [SerializeField] private int powerAmount;

    public int PowerAmount => powerAmount;

    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += EffectNewCards;
        EffectCardsAlreadyOnLane();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        
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
