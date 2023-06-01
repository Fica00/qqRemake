public class AddSameCardToHandOnReveal : CardSpecialEffectBase
{
    public override void Subscribe()
    {
        AddCardToHand();
    }

    void AddCardToHand()
    {
        CardObject _drawnCard = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.BotPlayer;
        _player.AddCardToHand(_drawnCard);
    }
}
