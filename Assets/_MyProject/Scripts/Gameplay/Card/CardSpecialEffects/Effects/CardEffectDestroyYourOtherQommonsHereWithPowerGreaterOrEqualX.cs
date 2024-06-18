using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectDestroyYourOtherQommonsHereWithPowerGreaterOrEqualX : CardEffectBase
{
    [SerializeField] private int powerX;

    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        if (GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects < 1)
        {
            return;
        }
        DestroyQommons();
    }

    void DestroyQommons()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _cardsToBeDestroyed = new List<CardObject>();
        List<CardObject> _oppoentsCardsOnLine = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);

        if (_oppoentsCardsOnLine.Count == 0)
        {
            return;
        }

        foreach (var _cardOnLane in _oppoentsCardsOnLine)
        {
            if (_cardOnLane.Stats.Power >= powerX) 
            {
                _cardsToBeDestroyed.Add(_cardOnLane);
                Debug.Log("!!!!!!!!!!!!!!!!!!Add card to destroy!!!!!!!!!!!!!!!!!!!");
            }
        }

        if (_cardsToBeDestroyed.Count == 0)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!No cards to destroy!!!!!!!!!!!!!!!!!!!");
            return;
        }
        
        if (GameplayManager.IsPvpGame)
        {
            ((GameplayManagerPVP)GameplayManager.Instance).TellOpponentToDestroyCardsOnTable(_cardsToBeDestroyed, false);
        }

        GameplayPlayer _opponentPlayer = cardObject.IsMy ? GameplayManager.Instance.OpponentPlayer : GameplayManager.Instance.MyPlayer;
        foreach (var _card in _cardsToBeDestroyed)
        {
            _opponentPlayer.DestroyCardFromTable(_card);
        }
    }
}