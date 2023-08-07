using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddNPowerToYourQommonsTable : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    
    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckCard;
        CheckQommonsThatAreAlreadyHere();
    }

    private void CheckQommonsThatAreAlreadyHere()
    {
        GameplayPlayer _player =
            cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        List<CardObject> _cardsOnTable = GameplayManager.Instance.TableHandler.GetCards(_player);

        foreach (var _cardOnLane in _cardsOnTable)
        {
            for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
            {
                _cardOnLane.Stats.Power += amountOfPower;
            }
            _cardOnLane.Display.EnlargedPowerAnimation(_cardOnLane.IsMy);
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
        
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            _card.Stats.Power += amountOfPower;
        }
        
        _card.Display.EnlargedPowerAnimation(_card.IsMy);
    }
}
