using Photon.Pun;
using UnityEngine;

public class CardEffectWhenDestroyedReturnToHand : CardEffectBase
{
    public override void Subscribe()
    {
        //nothing to do here
    }

    private void OnEnable()
    {
        GameplayPlayer.DestroyedCardFromTable += CheckCard;
    }

    private void OnDisable()
    {
        GameplayPlayer.DestroyedCardFromTable -= CheckCard;
    }

    private void CheckCard(CardObject _card)
    {
        if (GameplayManager.IsPvpGame && !_card.IsMy)
        {
            return;
        }

        if (_card == cardObject)
        {
            AddToHand();
        }
    }

    private void AddToHand()
    {
        CardObject _card = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;

        if ((!_player.IsMy) && (PhotonNetwork.CurrentRoom != null))
        {
            return;
        }

        _player.AddCardToHand(_card);
    }
}
