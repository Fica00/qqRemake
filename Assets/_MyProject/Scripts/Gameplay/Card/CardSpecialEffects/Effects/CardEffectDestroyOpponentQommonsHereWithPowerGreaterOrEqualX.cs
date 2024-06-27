using System.Collections.Generic;
using UnityEngine;

public class CardEffectDestroyOpponentQommonsHereWithPowerGreaterOrEqualX : CardEffectBase
{
    [SerializeField] private int powerX;

    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            DestroyQommons();
        }
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
            }
        }

        if (_cardsToBeDestroyed.Count == 0)
        {
            return;
        }

        if (GameplayManager.IsPvpGame)
        {
            GameplayManager.Instance.DestroyCardsOnTable(_cardsToBeDestroyed, false);
        }
        else 
        {
            GameplayPlayer _opponentPlayer = cardObject.IsMy ? GameplayManager.Instance.OpponentPlayer : GameplayManager.Instance.MyPlayer;
            foreach (var _card in _cardsToBeDestroyed)
            {
                _opponentPlayer.DestroyCardFromTable(_card);
            }
        }
    }
}