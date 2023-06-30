using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerIfYouDontPlayHereNextTurn : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    [SerializeField] private Color colorEffect;

    private bool shoudlDestroy = false;
    private bool shouldAddPower=true;


    public override void Subscribe()
    {
        GameplayManager.UpdatedGameState += SubscribeForEventsOnNextRound;
    }

    private void SubscribeForEventsOnNextRound()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                if (shoudlDestroy)
                {
                    if (shouldAddPower)
                    {
                        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
                        {
                            cardObject.Stats.Power += powerToAdd;
                        }
                    }
                    GameplayManager.UpdatedGameState -= SubscribeForEventsOnNextRound;
                    TableHandler.OnRevealdCard -= CheckPlayedCard;
                    GameplayManager.Instance.HideHighlihtWholeLocation(cardObject.LaneLocation, cardObject.IsMy, colorEffect);
                    Destroy(gameObject);
                }
                else
                {
                    TableHandler.OnRevealdCard += CheckPlayedCard;
                    shoudlDestroy = true;
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
            shouldAddPower = false;
            TableHandler.OnRevealdCard -= CheckPlayedCard;
        }
    }
}