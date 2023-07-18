using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddNPowerForEachQoomonInHand : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    
    public override void Subscribe()
    {
        CountCardsInHand();
        GameplayPlayer.DrewCard += CountCardsInHand;
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
        
        cardObject.Stats.Power = cardObject.Details.Power + _powerToAdd;
        (GameplayManagerPVP.Instance as GameplayManagerPVP).TellOpponentToAddPowerToQommons(
            new List<CardObject>(){cardObject},
            _powerToAdd);
    }
}
