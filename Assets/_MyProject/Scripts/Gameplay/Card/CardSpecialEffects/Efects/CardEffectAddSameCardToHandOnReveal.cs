using Photon.Pun;

public class CardEffectAddSameCardToHandOnReveal : CardEffectBase
{
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            AddCardToHand();
        }
    }

    void AddCardToHand()
    {
        CardObject _drawnCard = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        if ((!_player.IsMy) && (PhotonNetwork.CurrentRoom != null))
        {
            return;
        }
        _player.AddCardToHand(_drawnCard);
    }
}
