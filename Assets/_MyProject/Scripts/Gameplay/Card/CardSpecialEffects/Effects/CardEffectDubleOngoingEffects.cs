using System.Collections.Generic;
using UnityEngine;

public class CardEffectDubleOngoingEffects :  CardEffectBase
{
    [SerializeField] private HaloRingEffect haloRingEffect;
    
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        _currentLane.LaneSpecifics.AmountOfOngoingEffects *= 2;
        
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);
        List<CardObject> _opponentCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);

        foreach (var _card in _myCardsOnLane)
        {
            Instantiate(haloRingEffect, _card.transform);
        }

        foreach (var _card in _opponentCardsOnLane)
        {
            Instantiate(haloRingEffect, _card.transform);
        }
    }
}