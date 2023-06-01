using UnityEngine;

public class AddPowerIfYouPlayAnotherCardHereNextTurn : CardSpecialEffectBase
{
    [SerializeField] int PowerToAdd;
    bool shoudlDestroy = false;

    public override void Subscribe()
    {
        GameplayManager.UpdatedGameState += SubscribeForEventsOnNextRound;
    }

    void SubscribeForEventsOnNextRound()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                if (shoudlDestroy)
                {
                    GameplayManager.UpdatedGameState -= SubscribeForEventsOnNextRound;
                    TableHandler.OnRevealdCard -= CheckPlayedCard;
                    Destroy(gameObject);
                }
                else
                {
                    TableHandler.OnRevealdCard += CheckPlayedCard;
                    shoudlDestroy = true;
                }
                break;
            default:
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
            cardObject.Stats.Power += PowerToAdd;
            TableHandler.OnRevealdCard -= CheckPlayedCard;
            //todo show special effect
        }
    }
}
