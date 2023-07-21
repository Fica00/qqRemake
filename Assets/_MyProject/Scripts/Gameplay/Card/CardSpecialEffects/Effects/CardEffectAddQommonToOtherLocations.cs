using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddQommonToOtherLocations : CardEffectBase
{
    [SerializeField] private int qommonId;

    public override void Subscribe()
    {
        for (int i = 0;
             i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects;
             i++)
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

        List<LaneDisplay> _choosenLanes = new List<LaneDisplay>();

        switch (cardObject.LaneLocation)
        {
            case LaneLocation.Top:
                _choosenLanes.Add(GameplayManager.Instance.Lanes[1]);
                _choosenLanes.Add(GameplayManager.Instance.Lanes[2]);
                break;
            case LaneLocation.Mid:
                _choosenLanes.Add(GameplayManager.Instance.Lanes[0]);
                _choosenLanes.Add(GameplayManager.Instance.Lanes[2]);
                break;
            case LaneLocation.Bot:
                _choosenLanes.Add(GameplayManager.Instance.Lanes[0]);
                _choosenLanes.Add(GameplayManager.Instance.Lanes[1]);
                break;
            case LaneLocation.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        foreach (var _lane in _choosenLanes)
        {
            if (_lane.GetPlaceLocation(cardObject.IsMy) == null)
            {
                continue;
            }
            
            bool _shouldSkip = false;
            if (GameplayManager.Instance.LaneAbilities.ContainsKey(_lane))
            {
                var _laneAbility =
                    GameplayManager.Instance.LaneAbilities[_lane];
                if (_laneAbility != null)
                {
                    foreach (var _laneEffect in _laneAbility.Abilities)
                    {
                        if (_laneEffect is LaneAbilityOnlyXQommonsCanBePlacedHere)
                        {
                            _shouldSkip = true;
                            break;
                        }
                    }
                }
            }

            if (_shouldSkip)
            {
                continue;
            }

            if (_lane.CanPlace(cardObject))
            {
                CardObject _copyOfCard = CardsManager.Instance.CreateCard(qommonId, true);
                _copyOfCard.ForcePlace(_lane);
            }
        }
    }
}
