using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddNPowerForEachQoomonInHand : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    private GameplayPlayer player;
    
    public override void Subscribe()
    {
        player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        CountCardsInHand();
        GameplayPlayer.DrewCard += CountCardsInHand;
        GameplayPlayer.AddedCardToTable += CountCardsInHand;
        GameplayPlayer.DiscardedCard += CountCardsInHand;
        player.RemovedCardFromHand += CountCardsInHand;
        player.AddedCardToHand += CountCardsInHand;
        isSubscribed = true;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        
        GameplayPlayer.DrewCard -= CountCardsInHand;
        GameplayPlayer.AddedCardToTable -= CountCardsInHand;
        GameplayPlayer.DiscardedCard -= CountCardsInHand;
        player.RemovedCardFromHand -= CountCardsInHand;
        player.AddedCardToHand -= CountCardsInHand;
    }

    private void CountCardsInHand(CardObject _arg1, bool _arg2)
    {
        CountCardsInHand();
    }

    private void CountCardsInHand(PlaceCommand _obj)
    {
        CountCardsInHand();
    }

    private void CountCardsInHand(CardObject _card)
    {
        CountCardsInHand();
    }

    private void CountCardsInHand()
    {
        if (GameplayManager.IsPvpGame&&!cardObject.IsMy)
        {
            return;
        }

        int _amountOfCardsInHand =
            cardObject.IsMy ? 
                GameplayManager.Instance.MyPlayer.AmountOfCardsInHand :
                GameplayManager.Instance.OpponentPlayer.AmountOfCardsInHand;
        int _powerToAdd = 0;

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            _powerToAdd += amountOfPower;
        }

        _powerToAdd *= _amountOfCardsInHand;
        cardObject.Stats.Power = cardObject.Details.Power + _powerToAdd;
        if (GameplayManager.IsPvpGame)
        {
            (GameplayManagerPVP.Instance as GameplayManagerPVP).TellOpponentToAddPowerToQommons(
                new List<CardObject>(){cardObject},
                _powerToAdd);
        }
    }
}
