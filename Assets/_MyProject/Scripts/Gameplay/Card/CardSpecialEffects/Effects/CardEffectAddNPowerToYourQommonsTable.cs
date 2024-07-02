using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddNPowerToYourQommonsTable : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    private Dictionary<CardObject, int> changes = new();

    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += CheckCard;
        CheckQommonsThatAreAlreadyHere();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }

        TableHandler.OnRevealdCard -= CheckCard;
    }

    private void CheckQommonsThatAreAlreadyHere()
    {
        GameplayPlayer _player =
            cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        List<CardObject> _cardsOnTable = GameplayManager.Instance.TableHandler.GetCards(_player);

        foreach (var _cardOnLane in _cardsOnTable)
        {
            AddPowerToCard(_cardOnLane);
        }
    }

    private void CheckCard(CardObject _card)
    {
        if (_card.IsMy!=cardObject.IsMy)
        {
            return;
        }

        if (_card==cardObject)
        {
            return;
        }
        
        AddPowerToCard(_card);
    }

    private void AddPowerToCard(CardObject _card)
    {
        if (changes.ContainsKey(_card))
        {
            _card.Stats.Power -= changes[_card];
            changes.Remove(_card);
        }

        int _powerToAdd = 0;
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy); _i++)
        {
            _powerToAdd += amountOfPower;
        }
        changes.Add(_card, _powerToAdd);
        _card.Stats.Power += _powerToAdd;
        _card.Display.EnlargedPowerAnimation(_card.IsMy);
    }
}
