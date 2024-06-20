using UnityEngine;

public class CardEffectWhenDiscardedAddCardsToHand : CardEffectBase
{
    [SerializeField] private int amount;
    [SerializeField] private int cost;

    public override void Subscribe()
    {
        //nothing to do here
    }

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
        for (int i = 0; i < amount; i++)
        {
            CardObject _copyOfCard = CardsManager.Instance.CreateCard(cardObject.Details.Id, cardObject.IsMy);
            _copyOfCard.Stats.Energy = cost;

            GameplayManager.Instance.MyPlayer.AddCardToHand(_copyOfCard, true);
        }
    }
}