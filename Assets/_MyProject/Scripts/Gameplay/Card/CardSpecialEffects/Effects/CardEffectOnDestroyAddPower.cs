using System.Collections.Generic;
using UnityEngine;

public class CardEffectOnDestroyAddPower : CardEffectBase
{
    [SerializeField] private int power;

    public override void Subscribe()
    {
        //nothing to do here
    }

    private void OnEnable()
    {
        GameplayPlayer.DestroyedCardFromTable += CheckCard;
    }

    private void OnDisable()
    {
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
            AddPower();
        }
    }

    private void AddPower() 
    {
        List<CardObject> _addPowerList = new List<CardObject>();

        _addPowerList.AddRange(GameplayManager.Instance.MyPlayer.CardsOnBot);
        _addPowerList.AddRange(GameplayManager.Instance.MyPlayer.CardsOnMid);
        _addPowerList.AddRange(GameplayManager.Instance.MyPlayer.CardsOnTop);

        foreach (var _card in _addPowerList)
        {
            _card.Stats.Power += power;
        }
    }
}
