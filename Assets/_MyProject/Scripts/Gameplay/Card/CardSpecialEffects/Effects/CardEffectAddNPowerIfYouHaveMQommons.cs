using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddNPowerIfYouHaveMQommons : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    [SerializeField] private int amountOfQommons;

    private bool appliedPower;
    private int amountOfAppliedPower;

    public override void Subscribe()
    {
        CountCards();
        TableHandler.OnRevealdCard += CountCards;
    }

    private void CountCards(CardObject _card)
    {
        if (!_card.IsMy)
        {
            return;
        }
        CountCards();
    }
    
    void CountCards()
    {
        List<CardObject> _cardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation).ToList();

        if (_cardsOnLane.Contains(cardObject))
        {
            _cardsOnLane.Remove(cardObject);
        }

        if (_cardsOnLane.Count != amountOfQommons)
        {
            if (appliedPower)
            {
                appliedPower = false;
                cardObject.Stats.Power -= amountOfAppliedPower;
                amountOfAppliedPower = 0; // Reset the applied power when the condition is not met
            }
            return;
        }

        if (appliedPower)
        {
            return;
        }

        amountOfAppliedPower = 0;
        appliedPower = true;

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            amountOfAppliedPower += powerToAdd;
        }

        cardObject.Stats.Power += amountOfAppliedPower;
    }
}
