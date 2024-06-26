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

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            AddPowerToRandom();
        }
    }

    private void AddPowerToRandom()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _cardsInHand = GameplayManager.Instance.MyPlayer.CurrentCardsInHand;
        List<int> _randomCardsId = new List<int>();

        AddRandomCardsToList(_cardsInHand, _randomCardsId);

        GameplayManager.Instance.ChangeInMyHandRandomCardsPower(_randomCardsId, power,GameplayManager.Instance.MyPlayer);
    }

    private void AddRandomCardsToList(List<CardObject> _list, List<int> _newList)
    {
        List<CardObject> shuffledList = _list.OrderBy(x => Random.Range(0, _list.Count)).ToList();

        int addedCardsCount = 0;
        foreach (CardObject card in shuffledList)
        {
            if (!_newList.Contains(card.Details.Id))
            {
                _newList.Add(card.Details.Id);
                addedCardsCount++;
                if (addedCardsCount >= numberOfQoomons)
                {
                    break;
                }
            }
        }
    }
}