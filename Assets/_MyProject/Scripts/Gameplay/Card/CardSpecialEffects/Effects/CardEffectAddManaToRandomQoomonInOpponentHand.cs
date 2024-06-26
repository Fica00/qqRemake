using UnityEngine;

public class CardEffectAddManaToRandomQoomonInOpponentHand : CardEffectBase
{
    [SerializeField] private int manaToAdd;
    [SerializeField] private int manaLessThan;

    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            AddManaToRandom();
        }
    }

    private void AddManaToRandom()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.OpponentPlayer:GameplayManager.Instance.MyPlayer;
        GameplayManager.Instance.ChangeInOpponentHandRandomCardEnergy(manaLessThan, manaToAdd, _player);
    }
}