using System.Collections.Generic;
using UnityEngine;

public class CardEffectOnDestroyAddPower : CardEffectBase
{
    [SerializeField] private int power;

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
        // lista z
        List<CardObject> _addPowerList = new List<CardObject>();

        foreach (var _card in _addPowerList)
        {
            _card.Stats.Power += power;
        }
    }
}
