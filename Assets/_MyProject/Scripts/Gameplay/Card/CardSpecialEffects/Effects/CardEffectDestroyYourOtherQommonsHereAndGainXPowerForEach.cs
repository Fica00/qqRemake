using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectDestroyYourOtherQommonsHereAndGainXPowerForEach : CardEffectBase
{
    [SerializeField] private int powerPerQommon;
    
    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        if (GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects<1)
        {
            return;
        }
        DestroyQommons();
    }

    void DestroyQommons()
    {
        if (GameplayManager.IsPvpGame&& !cardObject.IsMy)
        {
            return;
        }
        
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);
        
        
        if (_myCardsOnLane.Contains(cardObject))
        {
            _myCardsOnLane.Remove(cardObject);
        }

        cardObject.Stats.Power += (_myCardsOnLane.Count * powerPerQommon);

        GameplayPlayer _myPlayer = cardObject.IsMy ?  GameplayManager.Instance.MyPlayer:GameplayManager.Instance.OpponentPlayer;
        foreach (var _card in _myCardsOnLane.ToList())
        {
            _myPlayer.DestroyCardFromTable(_card);
        }
    }
}
