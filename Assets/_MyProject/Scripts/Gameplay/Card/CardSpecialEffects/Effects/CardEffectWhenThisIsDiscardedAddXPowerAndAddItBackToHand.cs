using UnityEngine;

public class CardEffectWhenThisIsDiscardedAddXPowerAndAddItBackToHand : CardEffectBase
{
    [SerializeField] private int power;
    public override void Subscribe()
    {
        GameplayPlayer.DiscardedCard += CheckDiscardedCard;
    }

    private void OnDisable()
    {
        GameplayPlayer.DiscardedCard -= CheckDiscardedCard;
    }

    void CheckDiscardedCard(CardObject _card)
    {
        if (GameplayManager.IsPvpGame&&!_card.IsMy)
        {
            return;
        }

        if (_card==cardObject)
        {
            Apply();
        }
    }

    void Apply()
    {
        GameplayPlayer _player =
            cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        
        _player.AddCardToHand(cardObject);
    }
    
    
}
