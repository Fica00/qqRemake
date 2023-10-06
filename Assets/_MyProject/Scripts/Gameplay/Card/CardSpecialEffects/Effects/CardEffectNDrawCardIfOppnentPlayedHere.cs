using System;
using UnityEngine;

public class CardEffectNDrawCardIfOppnentPlayedHere : CardEffectBase
{
    [SerializeField] private int amountOfCards;
    private bool firstPhase;

    public override void Subscribe()
    {
        if (GameplayManager.IsPvpGame&&!cardObject.IsMy)
        {
            return;
        }
        
        GameplayManager.UpdatedGameState += SubscribeForNextRound;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }

        try
        {
            TableHandler.OnRevealdCard -= CheckCard;
        }
        catch 
        {
                        
        }
        try
        {
            GameplayManager.UpdatedGameState -= SubscribeForNextRound;
        }
        catch 
        {
                        
        }
    }

    private void SubscribeForNextRound()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                if (firstPhase)
                {
                    firstPhase = false;
                    try
                    {
                        TableHandler.OnRevealdCard -= CheckCard;
                    }
                    catch 
                    {
                        
                    }
                    GameplayManager.UpdatedGameState -= SubscribeForNextRound;
                }
                else
                {
                    TableHandler.OnRevealdCard += CheckCard;
                    firstPhase = true;
                }
                break;
            case GameplayState.Playing:
                break;
            case GameplayState.Waiting:
                break;
            case GameplayState.ResolvingEndOfRound:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckCard(CardObject _card)
    {
        if (GameplayManager.IsPvpGame&&!_card.IsMy)
        {
            return;
        }

        if (_card.IsMy==cardObject.IsMy)
        {
            return;
        }

        if (_card.LaneLocation!=cardObject.LaneLocation)
        {
            return;
        }
        
        _card.Display.ShowBladesEffect();
        
        for (int _j = 0;
             _j < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects;
             _j++)
        {
            if (cardObject.IsMy)
            {
                for (int _i = 0; _i < amountOfCards; _i++)
                {
                    GameplayManager.Instance.DrawCard(GameplayManager.Instance.MyPlayer);
                }
            }
            else if (!GameplayManager.IsPvpGame)
            {
                for (int _i = 0; _i < amountOfCards; _i++)
                {
                    GameplayManager.Instance.DrawCard(GameplayManager.Instance.OpponentPlayer);
                }
            }
        }
    }
    
}
