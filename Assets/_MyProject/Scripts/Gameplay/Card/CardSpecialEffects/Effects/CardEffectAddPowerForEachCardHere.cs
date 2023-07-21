using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddPowerForEachCardHere : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    
    public override void Subscribe()
    {
       AddPower();
    }

    void AddPower()
    {
        List<CardObject> _cardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation).ToList();

        if (_cardsOnLane.Contains(cardObject))
        {
            _cardsOnLane.Remove(cardObject);
        }
        
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            cardObject.Stats.Power += (_cardsOnLane.Count * powerToAdd);
        }

        foreach (var _cardOnLane in _cardsOnLane)
        {
            LanePlaceIdentifier _placeIdentifier = _cardOnLane.GetComponentInParent<LanePlaceIdentifier>();
            GameplayManager.Instance.FlashLocation(_placeIdentifier.Id,Color.white,3);
        }
    }
}
