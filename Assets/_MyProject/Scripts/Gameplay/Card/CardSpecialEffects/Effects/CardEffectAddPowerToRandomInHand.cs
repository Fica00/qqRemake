using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddPowerToRandomInHand : CardEffectBase
{
    [SerializeField] private int power;
    [SerializeField] private int numberOfQoomons;

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
        AddManaToRandom();
    }

    private void AddManaToRandom()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _cardsInHand = GameplayManager.Instance.MyPlayer.CurrentCardsInHand;
        List<CardObject> _randomCards = new List<CardObject>();

        AddRandomCardsToList(_cardsInHand, _randomCards);

        foreach (var _card in _randomCards)
        {
            _card.Stats.Power += power;
        }
    }

    private void AddRandomCardsToList(List<CardObject> _list, List<CardObject> _newList)
    {
        List<CardObject> shuffledList = _list.OrderBy(x => Random.Range(0, _list.Count)).ToList();

        int addedCardsCount = 0;
        foreach (CardObject card in shuffledList)
        {
            if (!_newList.Contains(card))
            {
                _newList.Add(card);
                addedCardsCount++;
                if (addedCardsCount >= numberOfQoomons)
                {
                    break;
                }
            }
        }
    }
}