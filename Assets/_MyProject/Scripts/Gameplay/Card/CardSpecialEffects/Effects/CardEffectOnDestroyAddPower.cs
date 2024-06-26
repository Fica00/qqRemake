using UnityEngine;

public class CardEffectOnDestroyAddPower : CardEffectBase
{
    [SerializeField] private int power;

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
        if (_card != cardObject)
        {
            return;
        }
        
        AddPower();
    }

    private void AddPower() 
    {
        var _cardsOnTable = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy);

        foreach (var _card in _cardsOnTable)
        {
            _card.Stats.Power += power;
        }
    }
}
