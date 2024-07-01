using UnityEngine;

public class CardEffectAddPowerOnDiscard : CardEffectBase
{
    [SerializeField] private int power;

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

        if (_card != cardObject)
        {
            GameplayManager.Instance.AddPowerToQoomon(cardObject.GetComponentInParent<LanePlaceIdentifier>().Id, power);
        }
    }
}