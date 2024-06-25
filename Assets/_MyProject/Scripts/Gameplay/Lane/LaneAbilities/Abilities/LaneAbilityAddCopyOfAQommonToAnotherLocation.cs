using System.Collections;
using System.Linq;
using UnityEngine;

public class LaneAbilityAddCopyOfAQommonToAnotherLocation : LaneAbilityBase
{
    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += AddCopyOfAQommon;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        
        TableHandler.OnRevealdCard -= AddCopyOfAQommon;
    }

    private void AddCopyOfAQommon(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }
        
        if (GameplayManager.IsPvpGame && !_card.IsMy)
        {
            return;
        }
        
        CardObject _copyOfCard = CardsManager.Instance.CreateCard(_card.Details.Id, _card.IsMy);
        LaneDisplay _choosendLane = null;
        int[] _randomIndexses = { 0, 1, 2 };
        _randomIndexses = _randomIndexses.OrderBy(_ => System.Guid.NewGuid()).ToArray();
        for (int _i = 0; _i < _randomIndexses.Length; _i++)
        {
            int _laneIndex = _randomIndexses[_i];
            
            if (_laneIndex == (int)laneDisplay.Location)
            {
                continue;
            }

            if (GameplayManager.Instance.Lanes[_laneIndex].GetPlaceLocation(_copyOfCard.IsMy) == null)
            {
                continue;
            }
            bool _shouldSkip = false;
            var _laneAbility = GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_laneIndex])?
                GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_laneIndex]]:
                null;
            
            if (_laneAbility!=null)
            {
                foreach (var _laneEffect in _laneAbility.Abilities)
                {
                    if (_laneEffect is LaneAbilityOnlyXQommonsCanBePlacedHere _limitationAbility)
                    {
                        _shouldSkip = true;
                        var _myQoomonsOnLane = GameplayManager.Instance.TableHandler.GetCards(_card.IsMy,GameplayManager.Instance.Lanes[_laneIndex]
                        .Location );
                        if (_myQoomonsOnLane.Count<_limitationAbility.AmountOfQommons)
                        {
                            _shouldSkip = false;
                        }
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

        if (_choosendLane == null)
        {
            Destroy(_copyOfCard.gameObject);
            return;
        }

        
        _copyOfCard.ForcePlace(_choosendLane);
        
        StartCoroutine(DelayPowerIcoliser(_copyOfCard.GetComponentInParent<LanePlaceIdentifier>().Id, _card.Stats.Power - _copyOfCard.Details
        .Power));
    }

    private IEnumerator DelayPowerIcoliser(int _placeId, int _power)
    {
        yield return new WaitForSeconds(1);
        GameplayManager.Instance.AddPowerOfQoomonOnPlace(_placeId,_power);
    }
}
