using UnityEngine;

public class CardEffectDiscardXHighestCostQommons : CardEffectBase
{
    [SerializeField] private int amountOfCards;
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            DiscardCard();
        }
    }

    void DiscardCard()
    {
        if (GameplayManager.IsPvpGame&& !cardObject.IsMy)
        {
            return;
        }

        for (int i = 0; i < amountOfCards; i++)
        {
            GameplayPlayer _player =
                cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
            CardObject _card = _player.GetHigestCostQommon();
            if (_card == null)
            {
                return;
            }

            _player.DiscardCardFromHand(_card);   
        }
    }
}
