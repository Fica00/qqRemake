using UnityEngine;

public class CardEffectWhenDiscardedAddCardsToHand : CardEffectBase
{
    [SerializeField] private int amount;
    [SerializeField] private int cost;

    private void OnEnable()
    {
        GameplayPlayer.DiscardedCard += CheckCard;
    }

    private void OnDisable()
    {
        GameplayPlayer.DiscardedCard -= CheckCard;
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
        // add amount of cards with this cost
    }
}
