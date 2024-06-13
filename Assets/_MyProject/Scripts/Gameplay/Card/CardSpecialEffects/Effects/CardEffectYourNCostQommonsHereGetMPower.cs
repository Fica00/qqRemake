using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectYourNCostQommonsHereGetMPower : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    [SerializeField] private int cost;
    
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
            if (_cardOnLane.Details.Mana!=cost)
            {
                continue;
            }
            
            for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
            {
                _cardOnLane.Stats.Power += amountOfPower;
            }
            _cardOnLane.Display.EnlargedPowerAnimation(_cardOnLane.IsMy);
            StartCoroutine(UpdatePower(_cardOnLane));
        }

        IEnumerator UpdatePower(CardObject _card)
        {
            yield return new WaitForSeconds(1);
            _card.Display.ForcePowerTextUpdateOnTable();
        }
    }

    private void CheckCard(CardObject _card)
    {
        if (_card.IsMy!=cardObject.IsMy)
        {
            return;
        }

        if (_card.Stats.Energy!=cost)
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
            _card.Display.EnlargedPowerAnimation(cardObject.IsMy);
        }
    }
}
