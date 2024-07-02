using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddQommonToOtherLocations : CardEffectBase
{
    [SerializeField] private int qommonId;

    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            Summon();
        }
    }

    private void Summon()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<LaneDisplay> _chosenLanes = new List<LaneDisplay>();

        switch (cardObject.LaneLocation)
        {
            case LaneLocation.Top:
                _chosenLanes.Add(GameplayManager.Instance.Lanes[1]);
                _chosenLanes.Add(GameplayManager.Instance.Lanes[2]);
                break;
            case LaneLocation.Mid:
                _chosenLanes.Add(GameplayManager.Instance.Lanes[0]);
                _chosenLanes.Add(GameplayManager.Instance.Lanes[2]);
                break;
            case LaneLocation.Bot:
                _chosenLanes.Add(GameplayManager.Instance.Lanes[0]);
                _chosenLanes.Add(GameplayManager.Instance.Lanes[1]);
                break;
            case LaneLocation.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        foreach (var _lane in _chosenLanes)
        {
            if (_lane.IsFull(cardObject))
            {
                continue;
            }
            
            CardObject _copyOfCard = CardsManager.Instance.CreateCard(qommonId, cardObject.IsMy);
            _copyOfCard.ForcePlace(_lane,false);
        }
    }
}
