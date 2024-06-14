public class CardEffectAddSameCardToHandOnReveal : CardEffectBase
{
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            AddCardToHand();
        }
    }

    private void AddCardToHand()
    {
        CardObject _drawnCard = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        if (SocketServerCommunication.Instance.MatchData != null || _player == null)
        {
            return;
        }
        _player.AddCardToHand(_drawnCard);
    }
}
