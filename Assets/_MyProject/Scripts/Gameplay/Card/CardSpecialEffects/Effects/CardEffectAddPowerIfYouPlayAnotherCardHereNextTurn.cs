using UnityEngine;

public class CardEffectAddPowerIfYouPlayAnotherCardHereNextTurn : CardEffectBase
{
    [SerializeField] private int PowerToAdd;
    [SerializeField] private Color colorEffect;

    private bool firstPhase;


    public override void Subscribe()
    {
        LaneDisplay _laneDisplay = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        if (_laneDisplay.CanPlaceAnyQommon())
        {
            GameplayManager.Instance.HighlihtWholeLocation(cardObject.LaneLocation, cardObject.IsMy, colorEffect);
        }
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
                    try
                    {
                        TableHandler.OnRevealdCard -= CheckPlayedCard;
                    }
                    catch
                    {
            
                    }
                    if (!cardObject.IsPlaced())
                    {
                        return;
                    }
                    
                    GameplayManager.Instance.HideHighlihtWholeLocation(cardObject.LaneLocation, cardObject.IsMy, colorEffect);
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
            for (int i = 0; i < GameplayManager.Instance.Lanes[(int)_cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
            {
                cardObject.Stats.Power += PowerToAdd;
            }
            TableHandler.OnRevealdCard -= CheckPlayedCard;
        }
    }
}