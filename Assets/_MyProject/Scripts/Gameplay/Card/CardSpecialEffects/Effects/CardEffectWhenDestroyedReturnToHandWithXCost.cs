using UnityEngine;

public class CardEffectWhenDestroyedReturnToHandWithXCost : CardEffectBase
{
    [SerializeField] private int cost;

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
        _card.Stats.Energy = cost;
        GameplayManager.Instance.MyPlayer.AddCardToHand(_card);
    }
}