using System.Collections.Generic;
using UnityEngine;

public class CardEffectDecreasePowerToOpponentsQommosHere : CardEffectBase
{
    [SerializeField] private int powerAmount;
   
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            DecreasePower();
        }
    }

    void DecreasePower()
    {
        List<CardObject> _opponentCards = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);

        foreach (var _opponentCard in _opponentCards)
        {
            _opponentCard.Stats.Power -= powerAmount;
        }
        
        GameplayManager.Instance.FlashWholeLocation(cardObject.LaneLocation,!cardObject.IsMy,Color.white,3);
    }
}
