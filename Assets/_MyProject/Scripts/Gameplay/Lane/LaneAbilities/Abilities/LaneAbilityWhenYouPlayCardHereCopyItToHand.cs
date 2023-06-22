public class LaneAbilityWhenYouPlayCardHereCopyItToHand : LaneAbilityBase
{
    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckCard;
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCard;
    }

    void CheckCard(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        if (_card.IsMy)
        {
            GameplayManager.Instance.MyPlayer.AddCardToHand(CardsManager.Instance.CreateCard(_card.Details.Id, _card.IsMy));
        }
        else if(!GameplayManager.IsPvpGame)
        {
            GameplayManager.Instance.OpponentPlayer.AddCardToHand(CardsManager.Instance.CreateCard(_card.Details.Id, _card.IsMy));
        }
    }
}
