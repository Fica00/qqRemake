using System.Collections.Generic;
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
        CountCards();
    }
    
    void CountCards()
    {
        List<CardObject> _cardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);

        if (_cardsOnLane.Contains(cardObject))
        {
            _cardsOnLane.Remove(cardObject);
        }
        if (_cardsOnLane.Count!=amountOfQommons)
        {
            if (appliedPower)
            {
                appliedPower = false;
                cardObject.Stats.Power -= amountOfAppliedPower;
            }
            return;
        }

        if (appliedPower)
        {
            return;
        }

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            amountOfAppliedPower += powerToAdd;
        }
        
        cardObject.Stats.Power += amountOfAppliedPower;
    }
}
