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

        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        GameplayManager.Instance.ChangeInMyHandRandomCardsPower(_randomCardsId, power,_player);
    }

    private void AddRandomCardsToList(List<CardObject> _list, List<int> _newList)
    {
        List<CardObject> _shuffledList = _list.OrderBy(_x => Random.Range(0, _list.Count)).ToList();

        int _addedCardsCount = 0;
        foreach (CardObject _card in _shuffledList)
        {
            if (_addedCardsCount >= numberOfQoomons)
            {
                break;
            }
            if (_newList.Contains(_card.Details.Id))
            {
                continue;
            }
            
            _newList.Add(_card.Details.Id);
            _addedCardsCount++;
        }
    }
}