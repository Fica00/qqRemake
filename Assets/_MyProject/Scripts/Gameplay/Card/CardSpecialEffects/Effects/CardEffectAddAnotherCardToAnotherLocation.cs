using System.Linq;
using UnityEngine;

public class CardEffectAddAnotherCardToAnotherLocation : CardEffectBase
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
        if (GameplayManager.IsPvpGame&&!cardObject.IsMy)
        {
            return;
        }
        
        CardObject _copyOfCard = CardsManager.Instance.CreateCard(qommonId, cardObject.IsMy);
        LaneDisplay _choosendLane = null;
        int[] _randomIndexses = new[] { 0, 1, 2 };
        _randomIndexses = _randomIndexses.OrderBy(_element => System.Guid.NewGuid()).ToArray();
        for (int _i = 0; _i < _randomIndexses.Length; _i++)
        {
            int _laneIndex = +_randomIndexses[_i];
            if (_laneIndex == (int)cardObject.LaneLocation)
            {
                continue;
            }
            if (GameplayManager.Instance.Lanes[_laneIndex].GetPlaceLocation(_copyOfCard.IsMy) != null)
            {
                bool _shouldSkip = false;
                if (!GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_laneIndex]))
                {
                    continue;
                }
                var _laneAbility = GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_laneIndex]];
                if (_laneAbility!=null)
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
              
                if (_shouldSkip)
                {
                    continue;
                }

                if (GameplayManager.Instance.Lanes[_laneIndex].CanPlace(_copyOfCard))
                {
                    _choosendLane = GameplayManager.Instance.Lanes[_laneIndex];
                    break;   
                }
            }
        }

        if (_choosendLane == null)
        {
            return;
        }

        _copyOfCard.ForcePlace(_choosendLane);
    }
}
