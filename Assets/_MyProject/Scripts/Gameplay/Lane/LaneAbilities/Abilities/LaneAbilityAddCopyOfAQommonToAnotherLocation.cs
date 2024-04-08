using System.Linq;

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
            
            if (GameplayManager.Instance.Lanes[_laneIndex].GetPlaceLocation(_copyOfCard.IsMy) != null)
            {
                bool _shouldSkip = false;
                var _laneAbility = GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_laneIndex])?
                    GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_laneIndex]]:
                    null;
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
