using System.Linq;

public class CardEffectOnDestroyOrDiscardResumonToRandomLane : CardEffectBase
{
    public override void Subscribe()
    {
        //nothing to do here
    }

    private void OnEnable()
    {
        GameplayPlayer.DiscardedCard += CheckCard;
        GameplayPlayer.DestroyedCardFromTable += CheckCard;
    }

    private void OnDisable()
    {
        GameplayPlayer.DiscardedCard -= CheckCard;
        GameplayPlayer.DestroyedCardFromTable -= CheckCard;
    }

    private void CheckCard(CardObject _card)
    {
        if (GameplayManager.IsPvpGame && !_card.IsMy)
        {
            return;
        }

        if (_card == cardObject)
        {
            SummonToRandomLane(_card);
        }
    }

    private void SummonToRandomLane(CardObject card)
    {
        CardObject _copyOfCard = CardsManager.Instance.CreateCard(card.Details.Id, cardObject.IsMy);
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
