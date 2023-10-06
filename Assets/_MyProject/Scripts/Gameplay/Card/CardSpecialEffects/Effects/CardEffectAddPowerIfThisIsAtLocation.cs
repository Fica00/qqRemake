using System;
using UnityEngine;

public class CardEffectAddPowerIfThisIsAtLocation : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    [SerializeField] private LaneLocation location;
    [SerializeField] private Color colorEffect;

    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckLocation;
        GameplayManager.UpdatedGameState += Destroy;
        isSubscribed = true;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckLocation;
        GameplayManager.UpdatedGameState -= Destroy;
    }

    private void Destroy()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                GameplayManager.UpdatedGameState -= Destroy;
                TableHandler.OnRevealdCard -= CheckLocation;
                Destroy(gameObject);
                break;
            case GameplayState.Playing:
                break;
            case GameplayState.Waiting:
                break;
            case GameplayState.ResolvingEndOfRound:
                break;
            default:
                break;
        }
    }

    private void CheckLocation(CardObject _cardObject)
    {
        if (_cardObject != cardObject)
        {
            return;
        }

        if (cardObject.LaneLocation == location)
        {
            for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
            {
                cardObject.Stats.Power += powerToAdd;
            }
            GameplayManager.Instance.FlashAllSpotsOnLocation(location, cardObject.IsMy, colorEffect, 2);
        }
    }
}
