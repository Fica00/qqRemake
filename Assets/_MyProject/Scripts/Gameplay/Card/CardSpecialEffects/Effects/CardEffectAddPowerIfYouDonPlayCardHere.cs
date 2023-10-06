using System;
using UnityEngine;

public class CardEffectAddPowerIfYouDonPlayCardHere : CardEffectBase
{
    [SerializeField] private int powerToAdd;

    private bool firstPhase = false;


    public override void Subscribe()
    {
        GameplayManager.Instance.HighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
        GameplayManager.UpdatedGameState += SubscribeForEventsOnNextRound;
    }

    private void OnDisable()
    {
        try
        {
            GameplayManager.UpdatedGameState -= SubscribeForEventsOnNextRound;
        }
        catch
        {
        }

        try
        {
            TableHandler.OnRevealdCard -= CheckPlayedCard;
        }
        catch
        {
        }
    }

    private void SubscribeForEventsOnNextRound()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                if (firstPhase)
                {
                    GameplayManager.UpdatedGameState -= SubscribeForEventsOnNextRound;
                    TableHandler.OnRevealdCard -= CheckPlayedCard;
                    GameplayManager.Instance.HideHighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
                    firstPhase = false;
                }
                else
                {
                    TableHandler.OnRevealdCard += CheckPlayedCard;
                    firstPhase = true;
                }
                break;
        }
    }

    private void CheckPlayedCard(CardObject _cardObject)
    {
        if (_cardObject.IsMy != cardObject.IsMy)
        {
            return;
        }
        
        if (_cardObject.LaneLocation == cardObject.LaneLocation)
        {
            TableHandler.OnRevealdCard -= CheckPlayedCard;
            return;
        }
        
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)_cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            cardObject.Stats.Power += powerToAdd;
        }
    }
}
