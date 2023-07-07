using UnityEngine;

public class CardEffectAddPowerIfYouDonPlayCardHere : CardEffectBase
{
    [SerializeField] private int powerToAdd;

    private bool shouldDestroy = false;


    public override void Subscribe()
    {
        GameplayManager.Instance.HighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
        GameplayManager.UpdatedGameState += SubscribeForEventsOnNextRound;
    }

    private void SubscribeForEventsOnNextRound()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                if (shouldDestroy)
                {
                    GameplayManager.UpdatedGameState -= SubscribeForEventsOnNextRound;
                    TableHandler.OnRevealdCard -= CheckPlayedCard;
                    GameplayManager.Instance.HideHighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
                    Destroy(gameObject);
                }
                else
                {
                    TableHandler.OnRevealdCard += CheckPlayedCard;
                    shouldDestroy = true;
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
