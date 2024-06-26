using UnityEngine;

public class CardEffectChangePowerInQoomonsOpponentHand : CardEffectBase
{
    [SerializeField] private int power;

    public override void Subscribe()
    {
        if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
        {
            return;
        }

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            ChangePower();
        }
    }

    private void ChangePower()
    {
        if (GameplayManager.IsPvpGame && !cardObject.IsMy)
        {
            return;
        }

        GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.OpponentPlayer:GameplayManager.Instance.MyPlayer;
        GameplayManager.Instance.ChangeAllInOpponentHandPower(power, _player);
    }
}
