using Photon.Pun;

public class CardEffectAddSameCardToHandOnReveal : CardEffectBase
{
    public override void Subscribe()
    {
        AddCardToHand();
    }

    void AddCardToHand()
    {
        CardObject _drawnCard = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        if ((!_player.IsMy)&&(PhotonNetwork.CurrentRoom!=null))
        {
            return;
        }
        _player.AddCardToHand(_drawnCard);
    }
}
